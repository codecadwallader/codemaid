/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VsSDK.UnitTestLibrary;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    /// <remarks>
    /// Part of VSIX unit testing starter kit.
    /// </remarks>
    internal class WindowFrameMock
    {
        private static GenericMockFactory _frameFactory;

        /// <summary>
        /// Return a IVsWindowFrame without any special implementation
        /// </summary>
        internal static IVsWindowFrame GetBaseFrame()
        {
            if (_frameFactory == null)
            {
                _frameFactory = new GenericMockFactory("WindowFrame", new[] { typeof(IVsWindowFrame), typeof(IVsWindowFrame2) });
            }

            var frame = (IVsWindowFrame)_frameFactory.GetInstance();
            return frame;
        }
    }
}