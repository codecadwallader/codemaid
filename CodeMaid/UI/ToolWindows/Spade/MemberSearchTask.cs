#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// A class implementing <see cref="VsSearchTask"/> in order to search code members.
    /// </summary>
    internal class MemberSearchTask : VsSearchTask
    {
        private readonly Action<string> _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberSearchTask" /> class.
        /// </summary>
        public MemberSearchTask(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback, Action<string> callback)
            : base(dwCookie, pSearchQuery, pSearchCallback)
        {
            _callback = callback;
        }

        /// <summary>
        /// Performs the search task.
        /// </summary>
        protected override void OnStartSearch()
        {
            ErrorCode = VSConstants.S_OK;

            try
            {
                _callback(SearchQuery.SearchString);
            }
            catch (Exception)
            {
                ErrorCode = VSConstants.E_FAIL;
            }

            base.OnStartSearch();
        }
    }
}