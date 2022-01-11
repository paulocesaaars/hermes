using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Deviot.Hermes.Application.Services
{
    public abstract class ServiceBase
    {
        protected readonly INotifier _notifier;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly IRepositorySQLite _repository;

        protected const string INTERNAL_ERROR_MESSAGE = "Houve um problema ao realizar o processamento";

        protected ServiceBase(INotifier notifier, ILogger logger, IMapper mapper, IRepositorySQLite repository)
        {
            _notifier = notifier;
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        protected virtual bool Validate<T>(IValidator<T> validator, T value)
        {
            var result = validator.Validate(value);
            if (result.IsValid)
                return true;

            foreach(var error in result.Errors)
                NotifyBadRequest(error.ErrorMessage);

            return false;
        }

        public virtual void NotifyOk(string message)
        {
            _logger.LogInformation(message);
            _notifier.Notify(HttpStatusCode.OK, message);
        }

        public virtual void NotifyCreated(string id, string message)
        {
            _logger.LogInformation(message);
            _notifier.Notify(id, HttpStatusCode.Created, message);
        }

        public virtual void NotifyNoContent(string message)
        {
            _logger.LogInformation(message);
            _notifier.Notify(HttpStatusCode.NoContent, message);
        }

        public virtual void NotifyBadRequest(string message)
        {
            _logger.LogWarning(message);
            _notifier.Notify(HttpStatusCode.BadRequest, message);
        }

        public virtual void NotifyForbidden(string message)
        {
            _logger.LogWarning(message);
            _notifier.Notify(HttpStatusCode.Forbidden, message);
        }

        public virtual void NotifyNotFound(string message)
        {
            _logger.LogWarning(message);
            _notifier.Notify(HttpStatusCode.NotFound, message);
        }

        public virtual void NotifyInternalServerError(Exception exception)
        {
            var messages = Utils.GetAllExceptionMessages(exception);

            foreach(var message in messages)
                _logger.LogError(message);

            _notifier.Notify(HttpStatusCode.InternalServerError, INTERNAL_ERROR_MESSAGE);
        }
    }
}
