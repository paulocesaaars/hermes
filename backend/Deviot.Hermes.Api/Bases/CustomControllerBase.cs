using Deviot.Common;
using Deviot.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Deviot.Hermes.Api.Bases
{
    [Authorize]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public abstract class CustomControllerBase : ControllerBase
    {
        protected readonly INotifier _notifier;
        protected readonly ILogger _logger;

        private const string CONTENT_TYPE = "application/json";
        private const string OK_MESSAGE = "A requisição foi executada com sucesso";
        private const string INVALID_ID = "Id inválido";
        private const string INTERNAL_ERROR_MESSAGE = "A requisição não foi executada com sucesso, erro não identificado";

        protected CustomControllerBase(INotifier notifier, 
                                       ILogger logger)
        {
            _notifier = notifier;
            _logger = logger;
        }

        private static string Serialize(CustomActionResult customActionResult)
        {
            return Utils.Serializer(customActionResult);
        }

        private static ContentResult GenerateContentResult(HttpStatusCode httpStatusCode, 
                                                           IEnumerable<string> messages, 
                                                           object data) 
        {
            var json = Serialize(new CustomActionResult(messages, data));

            return new ContentResult { StatusCode = (int) httpStatusCode, 
                                       Content = json, 
                                       ContentType = CONTENT_TYPE
                                     };
        }

        protected ActionResult CustomResponse(object value = null)
        {
            var messages = new List<string>();
            var httpStatusCode = HttpStatusCode.OK;
            
            if (_notifier.HasNotifications)
            {
                var notifies = _notifier.GetNotifications();

                if (notifies.Any(x => x.Type == HttpStatusCode.Unauthorized))
                {
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    messages.Add(notifies.First(x => x.Type == HttpStatusCode.Unauthorized).Message);
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.InternalServerError))
                {
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    messages.Add(notifies.First(x => x.Type == HttpStatusCode.InternalServerError).Message);
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.NotFound))
                {
                    httpStatusCode = HttpStatusCode.NotFound;
                    messages.Add(notifies.First(x => x.Type == HttpStatusCode.NotFound).Message);
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.Forbidden))
                {
                    httpStatusCode = HttpStatusCode.Forbidden;
                    messages.Add(notifies.First(x => x.Type == HttpStatusCode.Forbidden).Message);
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.BadRequest))
                {
                    httpStatusCode = HttpStatusCode.BadRequest;
                    foreach (var notify in notifies.Where(x => x.Type == HttpStatusCode.BadRequest))
                        messages.Add(notify.Message);
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.NoContent))
                {
                    return NoContent();
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.Created))
                {
                    httpStatusCode = HttpStatusCode.Created;
                    var notify = notifies.First(x => x.Type == HttpStatusCode.Created);
                    messages.Add(notify.Message);
                    Response.Headers.Location = $"{Url.RouteUrl(HttpContext.Request.RouteValues)}/{notify.Id}";
                }
                else if (notifies.Any(x => x.Type == HttpStatusCode.OK))
                {
                    httpStatusCode = HttpStatusCode.OK;
                    messages.Add(notifies.First(x => x.Type == HttpStatusCode.OK).Message);
                }
                else
                {
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    messages.Add(INTERNAL_ERROR_MESSAGE);
                }

                return GenerateContentResult(httpStatusCode, messages, value);
            }

            messages.Add(OK_MESSAGE);
            return GenerateContentResult(httpStatusCode, messages, value);
        }

        protected static ActionResult ReturnActionResultForInvalidId()
        {
            return GenerateContentResult(HttpStatusCode.BadRequest, new List<string>(1) { INVALID_ID }, null);
        }

        protected ActionResult ReturnActionResultForGenericError(Exception exception)
        {
            var errors = Utils.GetAllExceptionMessages(exception);
            foreach (var error in errors)
                _logger.LogError(error);

            return GenerateContentResult(HttpStatusCode.InternalServerError, new List<string>(1) { INTERNAL_ERROR_MESSAGE }, null);
        }
    }
}
