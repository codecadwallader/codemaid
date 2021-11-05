using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for text editor events.
    /// </summary>
    internal sealed class TextEditorEventListener : BaseEventListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        private TextEditorEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the text editor events, otherwise events will not register properly
            // via DTE.
            TextEditorEvents = Package.IDE.Events.TextEditorEvents;
        }

        /// <summary>
        /// An event raised when a line has been changed.
        /// </summary>
        internal event Action<Document> OnLineChanged;

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static TextEditorEventListener Instance { get; private set; }

        /// <summary>
        /// Gets or sets a pointer to the IDE text editor events.
        /// </summary>
        public TextEditorEvents TextEditorEvents { get; set; }

        /// <summary>
        /// Initializes a singleton instance of this event listener.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new TextEditorEventListener(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Registers event handlers with the IDE.
        /// </summary>
        protected override void RegisterListeners()
        {
            TextEditorEvents.LineChanged += TextEditorEvents_LineChanged;
        }

        /// <summary>
        /// Unregisters event handlers with the IDE.
        /// </summary>
        protected override void UnRegisterListeners()
        {
            TextEditorEvents.LineChanged -= TextEditorEvents_LineChanged;
        }

        /// <summary>
        /// An event handler for a line being changed.
        /// </summary>
        /// <param name="startPoint">The starting point of the change.</param>
        /// <param name="endPoint">The ending point of the change.</param>
        /// <param name="hint">A hint as to the type of change that has occurred.</param>
        private void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            var textDocument = startPoint?.Parent;
            if (textDocument == null) return;

            var document = startPoint.Parent.Parent;

            var onLineChanged = OnLineChanged;
            if (onLineChanged != null && document != null)
            {
                OutputWindowHelper.DiagnosticWriteLine($"TextEditorEventListener.OnLineChanged raised for '{document.FullName}'");

                onLineChanged(document);
            }
        }
    }
}