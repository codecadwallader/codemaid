#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// An options page for cleanup insert options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("8a2d94e1-f0c6-47c3-a894-127fd0602a68")]
    public class CleanupInsertOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupInsertOptionsPage"/> class.
        /// </summary>
        public CleanupInsertOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            InsertBlankLinePaddingBeforeUsingStatementBlocks = true;
            InsertBlankLinePaddingAfterUsingStatementBlocks = true;
            InsertBlankLinePaddingBeforeNamespaces = true;
            InsertBlankLinePaddingAfterNamespaces = true;
            InsertBlankLinePaddingBeforeRegionTags = true;
            InsertBlankLinePaddingAfterRegionTags = true;
            InsertBlankLinePaddingBeforeEndRegionTags = true;
            InsertBlankLinePaddingAfterEndRegionTags = true;
            InsertBlankLinePaddingBeforeClasses = true;
            InsertBlankLinePaddingAfterClasses = true;
            InsertBlankLinePaddingBeforeEnumerations = true;
            InsertBlankLinePaddingAfterEnumerations = true;
            InsertBlankLinePaddingBeforeEvents = true;
            InsertBlankLinePaddingAfterEvents = true;
            InsertBlankLinePaddingBeforeFieldsWithComments = true;
            InsertBlankLinePaddingAfterFieldsWithComments = true;
            InsertBlankLinePaddingBeforeInterfaces = true;
            InsertBlankLinePaddingAfterInterfaces = true;
            InsertBlankLinePaddingBeforeMethods = true;
            InsertBlankLinePaddingAfterMethods = true;
            InsertBlankLinePaddingBeforeProperties = true;
            InsertBlankLinePaddingAfterProperties = true;
            InsertBlankLinePaddingBeforeStructs = true;
            InsertBlankLinePaddingAfterStructs = true;
            InsertExplicitAccessModifiersOnClasses = true;
            InsertExplicitAccessModifiersOnEnumerations = true;
            InsertExplicitAccessModifiersOnEvents = true;
            InsertExplicitAccessModifiersOnInterfaces = true;
            InsertExplicitAccessModifiersOnMethods = true;
            InsertExplicitAccessModifiersOnProperties = true;
            InsertExplicitAccessModifiersOnStructs = true;
        }

        #endregion Constructors

        #region Public Properties

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before using statement blocks")]
        [Description("On cleanup code, inserts a single blank line of padding before a using statement block except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeUsingStatementBlocks { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after using statement blocks")]
        [Description("On cleanup code, inserts a single blank line of padding after a using statement block except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterUsingStatementBlocks { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before namespaces")]
        [Description("On cleanup code, inserts a single blank line of padding before a namespace except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeNamespaces { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after namespaces")]
        [Description("On cleanup code, inserts a single blank line of padding after a namespace except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterNamespaces { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before #region tags")]
        [Description("On cleanup code, inserts a single blank line of padding before a #region tag except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeRegionTags { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after #region tags")]
        [Description("On cleanup code, inserts a single blank line of padding after a #region tag except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterRegionTags { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before #endregion tags")]
        [Description("On cleanup code, inserts a single blank line of padding before a #endregion tag except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeEndRegionTags { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after #endregion tags")]
        [Description("On cleanup code, inserts a single blank line of padding after a #endregion tag except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterEndRegionTags { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before classes")]
        [Description("On cleanup code, inserts a single blank line of padding before a class except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeClasses { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after classes")]
        [Description("On cleanup code, inserts a single blank line of padding after a class except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterClasses { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before enumerations")]
        [Description("On cleanup code, inserts a single blank line of padding before an enumeration except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeEnumerations { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after enumerations")]
        [Description("On cleanup code, inserts a single blank line of padding after an enumeration except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterEnumerations { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before events")]
        [Description("On cleanup code, inserts a single blank line of padding before an event except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeEvents { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after events")]
        [Description("On cleanup code, inserts a single blank line of padding after an event except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterEvents { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before fields with comments")]
        [Description("On cleanup code, inserts a single blank line of padding before a field with a comment except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeFieldsWithComments { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after fields with comments")]
        [Description("On cleanup code, inserts a single blank line of padding after a field with a comment except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterFieldsWithComments { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before interfaces")]
        [Description("On cleanup code, inserts a single blank line of padding before an interface except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeInterfaces { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after interfaces")]
        [Description("On cleanup code, inserts a single blank line of padding after an interface except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterInterfaces { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before methods")]
        [Description("On cleanup code, inserts a single blank line of padding before a method except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeMethods { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after methods")]
        [Description("On cleanup code, inserts a single blank line of padding after a method except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterMethods { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before properties")]
        [Description("On cleanup code, inserts a single blank line of padding before a property except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeProperties { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after properties")]
        [Description("On cleanup code, inserts a single blank line of padding after a property except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterProperties { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding before structs")]
        [Description("On cleanup code, inserts a single blank line of padding before a struct except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingBeforeStructs { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert blank line padding after structs")]
        [Description("On cleanup code, inserts a single blank line of padding after a struct except where adjacent to a brace.")]
        public bool InsertBlankLinePaddingAfterStructs { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on classes")]
        [Description("On cleanup code, inserts explicit access modifiers on classes if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnClasses { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on enumerations")]
        [Description("On cleanup code, inserts explicit access modifiers on enumerations if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnEnumerations { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on events")]
        [Description("On cleanup code, inserts explicit access modifiers on events if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnEvents { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on interfaces")]
        [Description("On cleanup code, inserts explicit access modifiers on interfaces if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnInterfaces { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on methods")]
        [Description("On cleanup code, inserts explicit access modifiers on methods if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnMethods { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on properties")]
        [Description("On cleanup code, inserts explicit access modifiers on properties if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnProperties { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Insert explicit access modifiers on structs")]
        [Description("On cleanup code, inserts explicit access modifiers on structs if they are not specified.")]
        public bool InsertExplicitAccessModifiersOnStructs { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new CleanupInsertOptionsControl(this); }
        }

        #endregion Overrides
    }
}