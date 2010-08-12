#region CodeMaid is Copyright 2007-2010 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2010 Steve Cadwallader.

using System;
using System.Diagnostics;
using System.IO;

namespace RegistryIntegration
{
    /// <summary>
    /// A simple application to generate CodeMaid Wix fragments for registry integration.
    /// </summary>
    public class WixFragmentsGenerator
    {
        #region Public Methods

        /// <summary>
        /// The main application entry point.
        /// </summary>
        /// <param name="args">Arguments to the application.</param>
        public static void Main(string[] args)
        {
            try
            {
                string vs2008WixFragmentContent = GenerateVS2008WixFragment();
                DuplicateVS2005WixFragment(vs2008WixFragmentContent);
                DuplicateVS2010WixFragment(vs2008WixFragmentContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("----= ERROR =----");
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets the path to the RegPkg utility.
        /// </summary>
        /// <returns></returns>
        private static string GetRegPkgPath()
        {
            var vsSdkPath = Environment.GetEnvironmentVariable(VS_SDK_PATH_ENVIRONMENT_VARIABLE);

            if (String.IsNullOrEmpty(vsSdkPath))
            {
                throw new Exception(String.Format("Environment variable {0} is not defined.", VS_SDK_PATH_ENVIRONMENT_VARIABLE));
            }

            return vsSdkPath + @"VisualStudioIntegration\Tools\Bin\RegPkg.exe";
        }

        /// <summary>
        /// Generates the VS2008 wix fragment.
        /// </summary>
        /// <returns>The content of the generated VS2008 wix fragment.</returns>
        private static string GenerateVS2008WixFragment()
        {
            Console.WriteLine("----= Generating CodeMaidVS2008Registry WiX fragment =----");

            var process = new Process
                              {
                                  StartInfo =
                                      {
                                          UseShellExecute = false,
                                          CreateNoWindow = true,
                                          FileName = GetRegPkgPath(),
                                          Arguments =
                                              String.Format(@"/wixfile:{0} /codebase SteveCadwallader.CodeMaid.dll", CODEMAID_VS2008_WIX_FRAGMENT_PATH)
                                      }
                              };

            process.Start();
            process.WaitForExit();

            return File.ReadAllText(CODEMAID_VS2008_WIX_FRAGMENT_PATH);
        }

        /// <summary>
        /// Creates the VS2005 wix fragment from copying the VS2008 wix fragment.
        /// </summary>
        /// <param name="vs2008WixFragmentContent">The content of the VS2008 wix fragment.</param>
        private static void DuplicateVS2005WixFragment(string vs2008WixFragmentContent)
        {
            Console.WriteLine("----= Duplicating CodeMaidVS2005Registry WiX fragment =----");

            string vs2005WixFragmentContent = vs2008WixFragmentContent.Replace(VS2008_VERSION, VS2005_VERSION);

            File.WriteAllText(CODEMAID_VS2005_WIX_FRAGMENT_PATH, vs2005WixFragmentContent);
        }

        /// <summary>
        /// Creates the VS2010 wix fragment from copying the VS2008 wix fragment.
        /// </summary>
        /// <param name="vs2008WixFragmentContent">The content of the VS2008 wix fragment.</param>
        private static void DuplicateVS2010WixFragment(string vs2008WixFragmentContent)
        {
            Console.WriteLine("----= Duplicating CodeMaidVS2010Registry WiX fragment =----");

            string vs2010WixFragmentContent = vs2008WixFragmentContent.Replace(VS2008_VERSION, VS2010_VERSION);

            File.WriteAllText(CODEMAID_VS2010_WIX_FRAGMENT_PATH, vs2010WixFragmentContent);
        }

        #endregion Private Methods

        #region Private Constants

        /// <summary>
        /// The environment variable for the Visual Studio SDK path.
        /// </summary>
        private const string VS_SDK_PATH_ENVIRONMENT_VARIABLE = "VSSDK90Install";

        /// <summary>
        /// The path to the CodeMaid VS2005 WiX fragment.
        /// </summary>
        private const string CODEMAID_VS2005_WIX_FRAGMENT_PATH = @"..\..\Installer\CodeMaidVS2005Registry.wxi";

        /// <summary>
        /// The path to the CodeMaid VS2008 WiX fragment.
        /// </summary>
        private const string CODEMAID_VS2008_WIX_FRAGMENT_PATH = @"..\..\Installer\CodeMaidVS2008Registry.wxi";

        /// <summary>
        /// The path to the CodeMaid VS2010 WiX fragment.
        /// </summary>
        private const string CODEMAID_VS2010_WIX_FRAGMENT_PATH = @"..\..\Installer\CodeMaidVS2010Registry.wxi";

        /// <summary>
        /// The version number for Visual Studio 2005.
        /// </summary>
        private const string VS2005_VERSION = "8.0";

        /// <summary>
        /// The version number for Visual Studio 2008.
        /// </summary>
        private const string VS2008_VERSION = "9.0";

        /// <summary>
        /// The version number for Visual Studio 2010.
        /// </summary>
        private const string VS2010_VERSION = "10.0";

        #endregion Private Constants
    }
}