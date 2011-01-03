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

using System;
using System.Globalization;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// A static helper class for common options features.
    /// </summary>
    internal static class OptionsHelper
    {
        #region Internal Methods

        /// <summary>
        /// Gets the running IDE instance for the specified dialog page.
        /// </summary>
        /// <param name="dialogPage">The dialog page used to find the IDE.</param>
        /// <returns>The running IDE instance, otherwise null.</returns>
        internal static DTE2 GetIDE(DialogPage dialogPage)
        {
            if (dialogPage != null && dialogPage.Site != null)
            {
                return dialogPage.Site.GetService(typeof(DTE)) as DTE2;
            }

            return null;
        }

        /// <summary>
        /// Gets the version of the running IDE instance for the specified dialog page.
        /// </summary>
        /// <param name="dialogPage">The dialog page used to find the IDE.</param>
        /// <returns>The running IDE instance version, otherwise -1.</returns>
        internal static double GetIDEVersion(DialogPage dialogPage)
        {
            var dte = GetIDE(dialogPage);
            if (dte != null)
            {
                return Convert.ToDouble(dte.Version, CultureInfo.InvariantCulture);
            }

            return -1;
        }

        #endregion Internal Methods
    }
}