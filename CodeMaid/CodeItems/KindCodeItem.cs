#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// An enumeration of kind of code items.
    /// Does not 1:1 correspond to the code item classes (ex: CodeItemMethod includes constructors and destructors).
    /// </summary>
    public enum KindCodeItem
    {
        Constant,
        Field,
        Constructor,
        Destructor,
        Delegate,
        Event,
        Enum,
        Interface,
        Property,
        Method,
        Struct,
        Class,

        Namespace,
        Region,
        Using
    }
}