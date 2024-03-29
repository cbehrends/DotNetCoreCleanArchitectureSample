﻿using System;
using System.Linq;
using System.Security.Claims;

namespace Policy.Core.Model
{
    /// <summary>
    ///     Models an application role
    /// </summary>
    public class Role
    {

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        ///     Gets the Description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        internal bool Evaluate(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var roles = user.FindAll("role").Select(x => x.Value).ToList();
            return roles.Contains(Name);
        }
    }
}