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

using System;
using System.Diagnostics;
using System.Windows;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.About
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Event Handlers

        /// <summary>
        /// Called when an uncaptured left mouse button down event is received on the background.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Called when the Website link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnWebsiteLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"http://codemaid.net/");
        }

        /// <summary>
        /// Called when the Email link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnEmailLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"mailto:codemaid@gmail.com");
        }

        /// <summary>
        /// Called when the Twitter link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnTwitterLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"http://twitter.com/codemaid/");
        }

        /// <summary>
        /// Called when the RSS link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnRSSLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"http://codemaid.net/feed/");
        }

        /// <summary>
        /// Called when the Google+ link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnGooglePlusLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"https://plus.google.com/u/0/104818751344933095922/");
        }

        /// <summary>
        /// Called when the Trello link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnTrelloLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"https://trello.com/board/feature-ideas/4f6e6dcc255ed1e9085b8665");
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Attempts to launch the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        private static void LaunchLink(string link)
        {
            try
            {
                Process.Start(link);
            }
            catch (Exception)
            {
                // Do nothing if default application handler is not associated.
            }
        }

        #endregion Methods
    }
}