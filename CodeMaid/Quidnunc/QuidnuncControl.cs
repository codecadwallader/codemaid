#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The top level user control hosted within the quidnunc tool window.
    /// </summary>
    public partial class QuidnuncControl : UserControl
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncControl"/> class.
        /// </summary>
        public QuidnuncControl()
        {
            InitializeComponent();

            treeView.ImageList = GetImages();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        public Document Document
        {
            get { return _document; }
            set
            {
                if (_document != value)
                {
                    _document = value;

                    ClearTreeView();
                    BuildTreeView();
                }
            }
        }

        /// <summary>
        /// Gets or set a flag tracking if this control is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;

                    BuildTreeView();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Forces the control to refresh itself.
        /// </summary>
        public void ForceRefresh()
        {
            ClearTreeView();
            BuildTreeView();
        }

        /// <summary>
        /// Refreshes a CodeElement's corresponding tree node.
        /// </summary>
        /// <param name="element">The CodeElement to refresh.</param>
        public void Refresh(CodeElement element)
        {
            TreeNode node = FindAssociatedTreeNode(element);
            if (node != null)
            {
                // Refresh the underlying node.
                UpdateNode(node);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the tree view from the tracked document.
        /// </summary>
        private void BuildTreeView()
        {
            if (_isVisible && treeView.Nodes.Count == 0)
            {
                treeView.BeginUpdate();

                if (Document != null)
                {
                    try
                    {
                        var codeItems = CodeModelHelper.RetrieveAllCodeItems(Document);
                        var codeItemQ = new Queue<CodeItem>(codeItems);

                        CreateTree(codeItemQ);

                        if (treeView.Nodes.Count > 0)
                        {
                            treeView.ExpandAll();
                            treeView.Nodes[0].EnsureVisible();
                        }
                    }
                    catch (Exception) // Bad code formatting may cause crash.
                    {
                    }
                }

                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Clears the tree view.
        /// </summary>
        private void ClearTreeView()
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();
            treeView.EndUpdate();
        }

        /// <summary>
        /// Creates a TreeNode hierachy based on the sorted queue of code items.
        /// </summary>
        /// <param name="codeItemQ">The sorted queue of code items.</param>
        private void CreateTree(Queue<CodeItem> codeItemQ)
        {
            treeView.Nodes.Clear();

            if (codeItemQ.Count > 0)
            {
                // Process the first element in the list to initialize the process.
                CodeItem currentItem = codeItemQ.Dequeue();
                TreeNode currentNode = CreateNode(currentItem);
                treeView.Nodes.Add(currentNode);

                // Iterate over all remaining elements in the list.
                while (codeItemQ.Count > 0)
                {
                    CodeItem newItem = codeItemQ.Dequeue();

                    PlaceNode(ref currentItem, ref currentNode, newItem);
                }
            }
        }

        /// <summary>
        /// Creates TreeNodes and places them appropriately in the hierarchy.
        /// </summary>
        /// <param name="currentItem">The current item pointer.</param>
        /// <param name="currentNode">The current node pointer.</param>
        /// <param name="newItem">The new item to insert.</param>
        private void PlaceNode(ref CodeItem currentItem, ref TreeNode currentNode, CodeItem newItem)
        {
            if (newItem.StartLine < currentItem.EndLine)
            {
                // The new node is a child of the current node.
                TreeNode newNode = CreateNode(newItem);
                currentNode.Nodes.Add(newNode);
                currentNode = newNode;
                currentItem = newItem;
            }
            else
            {
                // The new node is not a child of the current node.  Move back up the hierarchy.
                currentNode = currentNode.Parent;

                if (currentNode == null)
                {
                    // We've hit the top of the hierarchy.
                    TreeNode newNode = CreateNode(newItem);
                    treeView.Nodes.Add(newNode);
                    currentNode = newNode;
                    currentItem = newItem;
                }
                else
                {
                    // Recurse back through at the higher level.
                    currentItem = (CodeItem)currentNode.Tag;
                    PlaceNode(ref currentItem, ref currentNode, newItem);
                }
            }
        }

        /// <summary>
        /// Creates a tree node for the given code item.
        /// </summary>
        /// <param name="item">The code item to create a tree node for.</param>
        /// <returns>The created tree node.</returns>
        private TreeNode CreateNode(CodeItem item)
        {
            TreeNode newNode = new TreeNode { Tag = item };

            UpdateNode(newNode);

            return newNode;
        }

        /// <summary>
        /// Updates the tree node's display and the underlying tagged code item.
        /// </summary>
        private void UpdateNode(TreeNode node)
        {
            CodeItem item = node.Tag as CodeItem;
            if (item != null)
            {
                // Refresh the associated code item if it is a CodeElement.
                CodeElement element = item.Object as CodeElement;
                if (element != null)
                {
                    item.Name = element.Name;
                    item.StartLine = element.StartPoint.Line;
                    item.EndLine = element.EndPoint.Line;
                }

                // Update the node images, text and tooltip.
                int imageIndex = GetImageIndex(item);

                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
                node.Text = item.Name;
                node.ToolTipText = item.Name;

                // Calculate and update complexity scores for functions and properties.
                if (element != null && ShouldShowComplexity(element))
                {
                    int complexity = CodeModelHelper.CalculateComplexity(element);
                    node.Text = String.Format("{0}   ({1})", element.Name, complexity);

                    int alertThreshold = Package != null
                            ? Package.Options.Snooper.ComplexityAlertThreshold
                            : 15;

                    int warningThreshold = Package != null
                            ? Package.Options.Snooper.ComplexityWarningThreshold
                            : 10;

                    if (complexity >= alertThreshold)
                    {
                        node.ForeColor = Color.Red;
                    }
                    else if (complexity >= warningThreshold)
                    {
                        node.ForeColor = Color.FromArgb(204, 71, 71);
                    }
                    else
                    {
                        node.ForeColor = Color.Black;
                    }
                }
                else
                {
                    node.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Searches the tree for a node with a CodeItem tag pointing to the
        /// given object.
        /// </summary>
        /// <param name="codeItemObject">The code item object to search for.</param>
        /// <returns>The matched tree node if found, null otherwise.</returns>
        private TreeNode FindAssociatedTreeNode(object codeItemObject)
        {
            return RecurseTree(treeView.Nodes,
                (node, objToMatch) =>
                {
                    CodeItem nodeItem = node.Tag as CodeItem;
                    if (nodeItem != null && nodeItem.Object == objToMatch)
                    {
                        return node;
                    }

                    return null;
                },
                codeItemObject) as TreeNode;
        }

        /// <summary>
        /// Jumps to the code item associated with the given node (if any).
        /// </summary>
        /// <param name="node">The node pointing to a code item where focus should move.</param>
        private void JumpToCode(TreeNode node)
        {
            try
            {
                if (node != null)
                {
                    CodeItem item = node.Tag as CodeItem;
                    if (item != null && _document != null)
                    {
                        // Update the node to make sure all information is current.
                        UpdateNode(node);

                        TextDocument textDoc = _document.Object("TextDocument") as TextDocument;
                        if (textDoc != null)
                        {
                            // Create an edit point at the starting location of the selected item.
                            EditPoint startPoint = textDoc.StartPoint.CreateEditPoint();
                            startPoint.MoveToLineAndOffset(item.StartLine, 1);

                            // Create a cloned point to find the closest preceeding blank line.
                            EditPoint blankPoint = startPoint.CreateEditPoint();
                            if (!blankPoint.AtStartOfDocument)
                            {
                                do
                                {
                                    blankPoint.LineUp(1);
                                } while (blankPoint.LineLength != 0 && !blankPoint.AtStartOfDocument);
                            }

                            // Move to the blank location, make it the top of the window and determine
                            // if the start location is on the screen.
                            textDoc.Selection.MoveToPoint(blankPoint, false);
                            bool startPointOnScreen = textDoc.Selection.ActivePoint.TryToShow(
                                vsPaneShowHow.vsPaneShowTop, startPoint);

                            // Move the cursor to the beginning of the text for the actual start point.
                            textDoc.Selection.MoveToPoint(startPoint, false);
                            textDoc.Selection.StartOfLine(vsStartOfLineOptions.vsStartOfLineOptionsFirstText, true);

                            if (!startPointOnScreen)
                            {
                                // Preceeding information is too large, just center on the starting point.
                                textDoc.Selection.ActivePoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, null);
                            }

                            // Cancel the selection, we don't actually want to highlight anything.
                            //TODO: Escaping out still seems to leave selection in odd state.
                            textDoc.Selection.Cancel();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // May be unable to navigate to nodes if they have been deleted without the snooper refreshing.
            }
        }

        #endregion Private Methods

        #region Private Static Methods

        /// <summary>
        /// Recurses across the given tree nodes, performing the given action and
        /// returning the result from the delegate when/if it is non-null.
        /// </summary>
        /// <param name="nodeList">The node collection to recursively analyze.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="actionParam">The parameter to pass into the action.</param>
        /// <returns>The first non-null action return, or null if none.</returns>
        private static object RecurseTree(TreeNodeCollection nodeList,
            Func<TreeNode, object, object> action, object actionParam)
        {
            object returnValue = null;

            if (nodeList != null)
            {
                foreach (TreeNode node in nodeList)
                {
                    // Check this node.
                    returnValue = action.Invoke(node, actionParam);
                    if (returnValue != null)
                    {
                        break;
                    }

                    // Check recursively the node's children.
                    returnValue = RecurseTree(node.Nodes, action, actionParam);
                    if (returnValue != null)
                    {
                        break;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Takes a given CodeItem and returns the associated image index.
        /// </summary>
        /// <param name="item">The CodeItem to analyze.</param>
        /// <returns>The associated image index.</returns>
        private static int GetImageIndex(CodeItem item)
        {
            int imageIndex = 0;

            CodeElement element = item.Object as CodeElement;
            if (element != null)
            {
                if (element.Kind == vsCMElement.vsCMElementNamespace)
                {
                    imageIndex = 2;
                }
                else
                {
                    vsCMAccess scope = vsCMAccess.vsCMAccessDefault;

                    // Determine base index.
                    switch (element.Kind)
                    {
                        case vsCMElement.vsCMElementClass:
                            imageIndex = 3;
                            scope = ((CodeClass)element).Access;
                            break;

                        case vsCMElement.vsCMElementInterface:
                            imageIndex = 8;
                            scope = ((CodeInterface)element).Access;
                            break;

                        case vsCMElement.vsCMElementStruct:
                            imageIndex = 13;
                            scope = ((CodeStruct)element).Access;
                            break;

                        case vsCMElement.vsCMElementEnum:
                            imageIndex = 18;
                            scope = ((CodeEnum)element).Access;
                            break;

                        case vsCMElement.vsCMElementDelegate:
                            imageIndex = 23;
                            scope = ((CodeDelegate)element).Access;
                            break;

                        case vsCMElement.vsCMElementEvent:
                            imageIndex = 28;
                            scope = ((CodeEvent)element).Access;
                            break;

                        case vsCMElement.vsCMElementFunction:
                            imageIndex = 33;
                            scope = ((CodeFunction)element).Access;
                            break;

                        case vsCMElement.vsCMElementProperty:
                            imageIndex = 38;
                            scope = ((CodeProperty)element).Access;
                            break;

                        case vsCMElement.vsCMElementVariable:
                            CodeVariable var = (CodeVariable)element;
                            imageIndex = !var.IsConstant ? 43 : 48;
                            scope = var.Access;
                            break;
                    }

                    // Offset for scope.
                    switch (scope)
                    {
                        case vsCMAccess.vsCMAccessPrivate: imageIndex += 4; break;
                        case vsCMAccess.vsCMAccessProtected: imageIndex += 3; break;
                        case vsCMAccess.vsCMAccessProject: imageIndex += 2; break;
                        case vsCMAccess.vsCMAccessAssemblyOrFamily: imageIndex += 1; break;
                    }
                }
            }
            else // If not a code element, assume code region.
            {
                imageIndex = 1;
            }

            return imageIndex;
        }

        /// <summary>
        /// Gets the images from resources.
        /// </summary>
        /// <returns>The image list.</returns>
        private static ImageList GetImages()
        {
            ImageList imageList = new ImageList
                                     {
                                         ColorDepth = ColorDepth.Depth32Bit,
                                         ImageSize = new Size(16, 16),
                                         TransparentColor = Color.Magenta
                                     };

            imageList.Images.Add(new Bitmap(16, 16));
            imageList.Images.Add(QuidnuncResources.Region);
            imageList.Images.Add(QuidnuncResources.Namespace);
            imageList.Images.Add(QuidnuncResources.Class);
            imageList.Images.Add(QuidnuncResources.ClassFriend);
            imageList.Images.Add(QuidnuncResources.ClassInternal);
            imageList.Images.Add(QuidnuncResources.ClassProtected);
            imageList.Images.Add(QuidnuncResources.ClassPrivate);
            imageList.Images.Add(QuidnuncResources.Interface);
            imageList.Images.Add(QuidnuncResources.InterfaceFriend);
            imageList.Images.Add(QuidnuncResources.InterfaceInternal);
            imageList.Images.Add(QuidnuncResources.InterfaceProtected);
            imageList.Images.Add(QuidnuncResources.InterfacePrivate);
            imageList.Images.Add(QuidnuncResources.Struct);
            imageList.Images.Add(QuidnuncResources.StructFriend);
            imageList.Images.Add(QuidnuncResources.StructInternal);
            imageList.Images.Add(QuidnuncResources.StructProtected);
            imageList.Images.Add(QuidnuncResources.StructPrivate);
            imageList.Images.Add(QuidnuncResources.Enum);
            imageList.Images.Add(QuidnuncResources.EnumFriend);
            imageList.Images.Add(QuidnuncResources.EnumInternal);
            imageList.Images.Add(QuidnuncResources.EnumProtected);
            imageList.Images.Add(QuidnuncResources.EnumPrivate);
            imageList.Images.Add(QuidnuncResources.Delegate);
            imageList.Images.Add(QuidnuncResources.DelegateFriend);
            imageList.Images.Add(QuidnuncResources.DelegateInternal);
            imageList.Images.Add(QuidnuncResources.DelegateProtected);
            imageList.Images.Add(QuidnuncResources.DelegatePrivate);
            imageList.Images.Add(QuidnuncResources.Event);
            imageList.Images.Add(QuidnuncResources.EventFriend);
            imageList.Images.Add(QuidnuncResources.EventInternal);
            imageList.Images.Add(QuidnuncResources.EventProtected);
            imageList.Images.Add(QuidnuncResources.EventPrivate);
            imageList.Images.Add(QuidnuncResources.Method);
            imageList.Images.Add(QuidnuncResources.MethodFriend);
            imageList.Images.Add(QuidnuncResources.MethodInternal);
            imageList.Images.Add(QuidnuncResources.MethodProtected);
            imageList.Images.Add(QuidnuncResources.MethodPrivate);
            imageList.Images.Add(QuidnuncResources.Property);
            imageList.Images.Add(QuidnuncResources.PropertyFriend);
            imageList.Images.Add(QuidnuncResources.PropertyInternal);
            imageList.Images.Add(QuidnuncResources.PropertyProtected);
            imageList.Images.Add(QuidnuncResources.PropertyPrivate);
            imageList.Images.Add(QuidnuncResources.Variable);
            imageList.Images.Add(QuidnuncResources.VariableFriend);
            imageList.Images.Add(QuidnuncResources.VariableInternal);
            imageList.Images.Add(QuidnuncResources.VariableProtected);
            imageList.Images.Add(QuidnuncResources.VariablePrivate);
            imageList.Images.Add(QuidnuncResources.Const);
            imageList.Images.Add(QuidnuncResources.ConstFriend);
            imageList.Images.Add(QuidnuncResources.ConstInternal);
            imageList.Images.Add(QuidnuncResources.ConstProtected);
            imageList.Images.Add(QuidnuncResources.ConstPrivate);

            return imageList;
        }

        /// <summary>
        /// Determines if the specified element should show a complexity score.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>True if complexity should be shown, false otherwise.</returns>
        private static bool ShouldShowComplexity(CodeElement element)
        {
            return element != null &&
                   (element.Kind == vsCMElement.vsCMElementFunction ||
                    element.Kind == vsCMElement.vsCMElementProperty);
        }

        #endregion Private Static Methods

        #region Private Event Handlers

        /// <summary>
        /// Handles the KeyPress event of the treeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        private void treeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') // Catch enter key presses
            {
                JumpToCode(treeView.SelectedNode);
            }
        }

        /// <summary>
        /// Handles the NodeMouseClick event of the treeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            JumpToCode(e.Node);
        }

        #endregion Private Event Handlers

        #region Private Fields

        /// <summary>
        /// The document currently tracked by the control.
        /// </summary>
        private Document _document;

        /// <summary>
        /// A flag tracking if the control is visible.
        /// </summary>
        private bool _isVisible;

        #endregion Private Fields
    }
}