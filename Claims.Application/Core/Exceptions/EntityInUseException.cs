using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Claims.Application.Core.Exceptions
{
    public class EntityInUseException : Exception
    {
        public EntityInUseException(): base("This entity is linked to others in the system and cannot be removed")
        {
            Errors = new Dictionary<string, string[]>();
        }
        
        public EntityInUseException(string message) : base(message)
        {
        }

        public EntityInUseException(string name, object key) : base($"\"{name}\" with Id ({key}) is linked to others in the system and cannot be removed.")
        {
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}