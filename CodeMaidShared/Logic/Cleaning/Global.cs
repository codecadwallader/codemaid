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
    public static class Global
    {
        static public AsyncPackage Package;

        static public T GetService<T>()
            => (T)Package?.GetServiceAsync(typeof(T))?.Result;

        static public DteDocument GetActiveDteDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            dynamic dte = GetService<EnvDTE.DTE>();
            return (DteDocument)dte.ActiveDocument;
        }

        static IVsStatusbar Statusbar;

        internal static void SetStatusMessage(string message)
        {
            if (Statusbar == null)
            {
                Statusbar = GetService<IVsStatusbar>();
                // StatusBar = Package.GetGlobalService(typeof(IVsStatusbar)) as IVsStatusbar;
            }

            Statusbar.SetText(message);
        }

        public static Document GetActiveDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Solution solution = Workspace.CurrentSolution;
            string activeDocPath = GetActiveDteDocument()?.FullName;

            if (activeDocPath != null)
                return solution.Projects
                               .SelectMany(x => x.Documents)
                               .FirstOrDefault(x => x.SupportsSyntaxTree &&
                                                    x.SupportsSemanticModel &&
                                                    x.FilePath == activeDocPath);
            return null;
        }

        private static VisualStudioWorkspace workspace = null;

        static public VisualStudioWorkspace Workspace
        {
            get
            {
                if (workspace == null)
                {
                    IComponentModel componentModel = GetService<SComponentModel>() as IComponentModel;
                    workspace = componentModel.GetService<VisualStudioWorkspace>();
                }
                return workspace;
            }
        }
    }

    internal class Class1
    {
        private protected Class1()
        {
        }
    }
}