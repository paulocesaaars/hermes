using System;
using System.ComponentModel.DataAnnotations;

namespace Deviot.Hermes.Application.ViewModels
{
    public abstract class EntityViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
