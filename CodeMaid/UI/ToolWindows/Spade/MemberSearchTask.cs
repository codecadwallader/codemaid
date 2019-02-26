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
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                _callback(SearchQuery.SearchString);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            }
            catch (Exception)
            {
                ErrorCode = VSConstants.E_FAIL;
            }

            base.OnStartSearch();
        }
    }
}