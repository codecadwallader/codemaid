#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// This attribute is used to declare that a property should raise a notification
    /// when an independent property is raising a notification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotifiesOnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotifiesOnAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the independent property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public NotifiesOnAttribute(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            Name = name;
        }

        /// <summary>
        /// The name of the independent property.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A unique identifier for this attribute.
        /// </summary>
        public override object TypeId
        {
            get { return this; }
        }
    }
}