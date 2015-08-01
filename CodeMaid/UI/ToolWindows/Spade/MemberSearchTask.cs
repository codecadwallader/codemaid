using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// Represents class for performing search tasks by members
    /// </summary>
    internal class MemberSearchTask : VsSearchTask
    {
        private readonly SpadeToolWindow _toolWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberSearchTask" /> class.
        /// </summary>
        public MemberSearchTask(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback, SpadeToolWindow toolWindow)
            : base(dwCookie, pSearchQuery, pSearchCallback)
        {
            _toolWindow = toolWindow;
        }

        /// <summary>
        /// Performs the search task.
        /// </summary>
        protected override void OnStartSearch()
        {
            ErrorCode = VSConstants.S_OK;

            try
            {
                _toolWindow.ViewModel.NameFilter = SearchQuery.SearchString;
                _toolWindow.ConditionallyUpdateCodeModel(false);
            }
            catch (Exception e)
            {
                ErrorCode = VSConstants.E_FAIL;
            }

            base.OnStartSearch();
        }
    }
}