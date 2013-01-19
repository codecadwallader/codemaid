#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Prompts
{
    /// <summary>
    /// A view model for providing a yes no prompt.
    /// </summary>
    public class YesNoPromptViewModel : ViewModelBase
    {
        #region Properties

        private string _title;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _message;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        private BitmapSource _iconSource;

        /// <summary>
        /// Gets the icon source.
        /// </summary>
        public BitmapSource IconSource
        {
            get { return _iconSource ?? (_iconSource = Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Question.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())); }
        }

        private bool _canRemember;

        /// <summary>
        /// Gets or sets a flag indicating if the result can be remembered.
        /// </summary>
        public bool CanRemember
        {
            get { return _canRemember; }
            set
            {
                if (_canRemember != value)
                {
                    _canRemember = value;
                    NotifyPropertyChanged("CanRemember");

                    if (!CanRemember)
                    {
                        Remember = false;
                    }
                }
            }
        }

        private bool _remember;

        /// <summary>
        /// Gets or sets a flag indicating if the result should be remembered.
        /// </summary>
        public bool Remember
        {
            get { return _remember; }
            set
            {
                if (_remember != value && (CanRemember || !value))
                {
                    _remember = value;
                    NotifyPropertyChanged("Remember");
                }
            }
        }

        private bool? _dialogResult;

        /// <summary>
        /// Gets or sets the dialog result.
        /// </summary>
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set
            {
                if (_dialogResult != value)
                {
                    _dialogResult = value;
                    NotifyPropertyChanged("DialogResult");
                }
            }
        }

        #endregion Properties

        #region SetDialogResult Command

        private DelegateCommand _setDialogResultCommand;

        /// <summary>
        /// Gets the set dialog result command.
        /// </summary>
        public DelegateCommand SetDialogResultCommand
        {
            get { return _setDialogResultCommand ?? (_setDialogResultCommand = new DelegateCommand(OnSetDialogResultCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="SetDialogResultCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSetDialogResultCommandExecuted(object parameter)
        {
            bool result;
            if (bool.TryParse(parameter as string, out result))
            {
                DialogResult = result;
            }
        }

        #endregion SetDialogResult Command
    }
}