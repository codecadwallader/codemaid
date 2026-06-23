using Microsoft.VisualStudio.Shell;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    internal static class UIThread
    {
        public static void Run(Action action)
        {
            if (ThreadHelper.CheckAccess())
            {
                action();
            }
            else
            {
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    action();
                });
            }
        }

        public static void RunOnUIThread(Action action)
        {
            Run(() =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                action();
            });
        }

        public static T Run<T>(Func<T> func)
        {
            if (ThreadHelper.CheckAccess())
            {
                return func();
            }

            return ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                return func();
            });
        }

        public static T RunOnUIThread<T>(Func<T> func)
        {
            return Run(() =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return func();
            });
        }
    }
}
