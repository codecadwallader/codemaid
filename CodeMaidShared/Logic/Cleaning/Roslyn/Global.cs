using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Linq;
using Document = Microsoft.CodeAnalysis.Document;
using DteDocument = EnvDTE.Document;
using Solution = Microsoft.CodeAnalysis.Solution;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    internal static class Global
    {
        public static AsyncPackage Package;

        public static T GetService<T>(AsyncPackage package)
            => (T)package?.GetServiceAsync(typeof(T))?.Result;

        public static DteDocument GetActiveDteDocument(AsyncPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            dynamic dte = GetService<EnvDTE.DTE>(package);
            return (DteDocument)dte.ActiveDocument;
        }

        static IVsStatusbar Statusbar;

        internal static void SetStatusMessage(AsyncPackage package, string message)
        {
            if (Statusbar == null)
            {
                Statusbar = GetService<IVsStatusbar>(package);
                // StatusBar = Package.GetGlobalService(typeof(IVsStatusbar)) as IVsStatusbar;
            }
            ThreadHelper.ThrowIfNotOnUIThread();
            Statusbar.SetText(message);
        }

        public static Document GetActiveDocument(AsyncPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Solution solution = GetWorkspace(package).CurrentSolution;
            string activeDocPath = GetActiveDteDocument(package)?.FullName;

            if (activeDocPath != null)
                return solution.Projects
                               .SelectMany(x => x.Documents)
                               .FirstOrDefault(x => x.SupportsSyntaxTree &&
                                                    x.SupportsSemanticModel &&
                                                    x.FilePath == activeDocPath);
            return null;
        }

        private static VisualStudioWorkspace workspace = null;

        public static VisualStudioWorkspace GetWorkspace(AsyncPackage package)
        {
            if (workspace == null)
            {
                IComponentModel componentModel = GetService<SComponentModel>(package) as IComponentModel;
                workspace = componentModel.GetService<VisualStudioWorkspace>();
            }
            return workspace;
        }
    }
}