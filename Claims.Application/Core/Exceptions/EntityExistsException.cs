using System;
using System.Collections.Generic;

namespace Claims.Application.Core.Exceptions
{
    public class EntityExistsException: Exception
    {
        public EntityExistsException(): base("This entity exists in the system and cannot be added again.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        
        public EntityExistsException(string message) : base($"\"{message}\"  exists in the system and cannot be added again.")
        {
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}