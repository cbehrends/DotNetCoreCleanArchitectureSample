using System;
using System.Collections.Generic;
using System.Linq;

namespace Policy.Core.Model
{
    /// <summary>
    /// Models a permission
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<string> Roles { get; } = new();

        internal bool Evaluate(IEnumerable<string> roles)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));

            return Roles.Any(roles.Contains);
        }
    }
}