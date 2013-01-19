#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.ComponentModel;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// An enumeration of kind of code items.
    /// Does not 1:1 correspond to the code item classes (ex: CodeItemMethod includes constructors and destructors).
    /// </summary>
    public enum KindCodeItem
    {
        [Description("Fields")]
        Field,

        [Description("Constructors")]
        Constructor,

        [Description("Destructors")]
        Destructor,

        [Description("Delegates")]
        Delegate,

        [Description("Events")]
        Event,

        [Description("Enums")]
        Enum,

        [Description("Indexers")]
        Indexer,

        [Description("Interfaces")]
        Interface,

        [Description("Properties")]
        Property,

        [Description("Methods")]
        Method,

        [Description("Structs")]
        Struct,

        [Description("Classes")]
        Class,

        [Description("Namespaces")]
        Namespace,

        [Description("Regions")]
        Region,

        [Description("Usings")]
        Using
    }
}