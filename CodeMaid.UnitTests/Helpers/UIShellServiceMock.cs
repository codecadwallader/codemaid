/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VsSDK.UnitTestLibrary;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    /// <remarks>
    /// Part of VSIX unit testing starter kit.
    /// </remarks>
    internal static class UIShellServiceMock
    {
        private static GenericMockFactory _uiShellFactory;

        #region UiShell Getters

        /// <summary>
        /// Returns an IVsUiShell that does not implement any methods
        /// </summary>
        /// <returns></returns>
        internal static BaseMock GetUiShellInstance()
        {
            if (_uiShellFactory == null)
            {
                _uiShellFactory = new GenericMockFactory("UiShell", new[] { typeof(IVsUIShell), typeof(IVsUIShellOpenDocument) });
            }

            BaseMock uiShell = _uiShellFactory.GetInstance();
            return uiShell;
        }

        /// <summary>
        /// Get an IVsUiShell that implement CreateToolWindow
        /// </summary>
        /// <returns>uishell mock</returns>
        internal static BaseMock GetUiShellInstanceCreateToolWin()
        {
            BaseMock uiShell = GetUiShellInstance();
            string name = $"{typeof(IVsUIShell).FullName}.{"CreateToolWindow"}";
            uiShell.AddMethodCallback(name, CreateToolWindowCallBack);

            return uiShell;
        }

        /// <summary>
        /// Get an IVsUiShell that implement CreateToolWindow (negative test)
        /// </summary>
        /// <returns>uishell mock</returns>
        internal static BaseMock GetUiShellInstanceCreateToolWinReturnsNull()
        {
            BaseMock uiShell = GetUiShellInstance();
            string name = $"{typeof(IVsUIShell).FullName}.{"CreateToolWindow"}";
            uiShell.AddMethodCallback(name, CreateToolWindowNegativeTestCallBack);

            return uiShell;
        }

        #endregion UiShell Getters

        #region Callbacks

        private static void CreateToolWindowCallBack(object caller, CallbackArgs arguments)
        {
            arguments.ReturnValue = VSConstants.S_OK;

            // Create the output mock object for the frame
            IVsWindowFrame frame = WindowFrameMock.GetBaseFrame();
            arguments.SetParameter(9, frame);
        }

        private static void CreateToolWindowNegativeTestCallBack(object caller, CallbackArgs arguments)
        {
            arguments.ReturnValue = VSConstants.S_OK;

            //set the windowframe object to null
            arguments.SetParameter(9, null);
        }

        #endregion Callbacks
    }
}