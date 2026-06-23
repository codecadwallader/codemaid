using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating insertion of explicit access modifier logic.
    /// </summary>
    internal class InsertExplicitAccessModifierLogic
    {
        #region Constants

        private const string PartialKeyword = "partial";

        #endregion Constants

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="InsertExplicitAccessModifierLogic" /> class.
        /// </summary>
        private static InsertExplicitAccessModifierLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="InsertExplicitAccessModifierLogic" /> class.
        /// </summary>
        /// <returns>An instance of the <see cref="InsertExplicitAccessModifierLogic" /> class.</returns>
        internal static InsertExplicitAccessModifierLogic GetInstance()
        {
            return _instance ?? (_instance = new InsertExplicitAccessModifierLogic());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertExplicitAccessModifierLogic" /> class.
        /// </summary>
        private InsertExplicitAccessModifierLogic()
        {
        }

        #endregion Constructors

        #region Insertion Methods

        /// <summary>
        /// Inserts the explicit access modifiers on classes where they are not specified.
        /// </summary>
        /// <param name="classes">The classes.</param>
        public void InsertExplicitAccessModifiersOnClasses(IEnumerable<CodeItemClass> classes)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses) return;

            foreach (var codeClass in classes.Select(x => x.CodeClass).Where(y => y != null))
            {
                var classDeclaration = CodeElementHelper.GetClassDeclaration(codeClass);

                // Skip partial classes - access modifier may be specified elsewhere.
                if (IsKeywordSpecified(classDeclaration, PartialKeyword))
                {
                    continue;
                }

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(classDeclaration, codeClass.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeClass.Access = codeClass.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on class {codeClass.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on delegates where they are not specified.
        /// </summary>
        /// <param name="delegates">The delegates.</param>
        public void InsertExplicitAccessModifiersOnDelegates(IEnumerable<CodeItemDelegate> delegates)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates) return;

            foreach (var codeDelegate in delegates.Select(x => x.CodeDelegate).Where(y => y != null))
            {
                var delegateDeclaration = CodeElementHelper.GetDelegateDeclaration(codeDelegate);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(delegateDeclaration, codeDelegate.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeDelegate.Access = codeDelegate.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on delegate {codeDelegate.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on enumerations where they are not specified.
        /// </summary>
        /// <param name="enumerations">The enumerations.</param>
        public void InsertExplicitAccessModifiersOnEnumerations(IEnumerable<CodeItemEnum> enumerations)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEnumerations) return;

            foreach (var codeEnum in enumerations.Select(x => x.CodeEnum).Where(y => y != null))
            {
                var enumDeclaration = CodeElementHelper.GetEnumerationDeclaration(codeEnum);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(enumDeclaration, codeEnum.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeEnum.Access = codeEnum.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on enum {codeEnum.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on events where they are not specified.
        /// </summary>
        /// <param name="events">The events.</param>
        public void InsertExplicitAccessModifiersOnEvents(IEnumerable<CodeItemEvent> events)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents) return;

            foreach (var item in events)
            {
                if (item?.CodeEvent == null) continue;
                var codeEvent = item.CodeEvent;

                try
                {
                    // Skip events defined inside an interface.
                    if (codeEvent.Parent is CodeInterface)
                    {
                        continue;
                    }

                    // Skip explicit interface implementations.
                    if (ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(codeEvent))
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Skip this event if unable to analyze.
                    continue;
                }

                var eventDeclaration = CodeElementHelper.GetEventDeclaration(codeEvent);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(eventDeclaration, codeEvent.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeEvent.Access = codeEvent.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on event {codeEvent.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on fields where they are not specified.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public void InsertExplicitAccessModifiersOnFields(IEnumerable<CodeItemField> fields)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnFields) return;

            foreach (var item in fields)
            {
                if (item?.CodeVariable == null) continue;
                var codeField = item.CodeVariable;

                try
                {
                    // Skip "fields" defined inside an enumeration.
                    if (codeField.Parent is CodeEnum)
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Skip this field if unable to analyze.
                    continue;
                }

                var fieldDeclaration = CodeElementHelper.GetFieldDeclaration(codeField);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(fieldDeclaration, codeField.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeField.Access = codeField.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on field {codeField.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on interfaces where they are not specified.
        /// </summary>
        /// <param name="interfaces">The interfaces.</param>
        public void InsertExplicitAccessModifiersOnInterfaces(IEnumerable<CodeItemInterface> interfaces)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces) return;

            foreach (var item in interfaces)
            {
                if (item?.CodeInterface == null) continue;
                var codeInterface = item.CodeInterface;

                var interfaceDeclaration = CodeElementHelper.GetInterfaceDeclaration(codeInterface);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(interfaceDeclaration, codeInterface.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeInterface.Access = codeInterface.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on interface {codeInterface.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on methods where they are not specified.
        /// </summary>
        /// <param name="methods">The methods.</param>
        public void InsertExplicitAccessModifiersOnMethods(IEnumerable<CodeItemMethod> methods)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods) return;

            foreach (var item in methods)
            {
                if (item?.CodeFunction == null) continue;
                var codeFunction = item.CodeFunction;

                try
                {
                    // Skip static constructors - they should not have an access modifier.
                    if (codeFunction.IsShared && codeFunction.FunctionKind == vsCMFunction.vsCMFunctionConstructor)
                    {
                        continue;
                    }

                    // Skip destructors - they should not have an access modifier.
                    if (codeFunction.FunctionKind == vsCMFunction.vsCMFunctionDestructor)
                    {
                        continue;
                    }

                    // Skip explicit interface implementations.
                    if (ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(codeFunction))
                    {
                        continue;
                    }

                    // Skip methods defined inside an interface.
                    if (codeFunction.Parent is CodeInterface)
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Skip this method if unable to analyze.
                    continue;
                }

                var methodDeclaration = CodeElementHelper.GetMethodDeclaration(codeFunction);

                // Skip partial methods - access modifier may be specified elsewhere.
                if (IsKeywordSpecified(methodDeclaration, PartialKeyword))
                {
                    continue;
                }

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(methodDeclaration, codeFunction.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeFunction.Access = codeFunction.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on method {codeFunction.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on properties where they are not specified.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public void InsertExplicitAccessModifiersOnProperties(IEnumerable<CodeItemProperty> properties)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties) return;

            foreach (var item in properties)
            {
                if (item?.CodeProperty == null) continue;
                var codeProperty = item.CodeProperty;

                try
                {
                    // Skip explicit interface implementations.
                    if (ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(codeProperty))
                    {
                        continue;
                    }

                    // Skip properties defined inside an interface.
                    if (codeProperty.Parent is CodeInterface)
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Skip this property if unable to analyze.
                    continue;
                }

                var propertyDeclaration = CodeElementHelper.GetPropertyDeclaration(codeProperty);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(propertyDeclaration, codeProperty.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeProperty.Access = codeProperty.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on property {codeProperty.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the explicit access modifiers on structs where they are not specified.
        /// </summary>
        /// <param name="structs">The structs.</param>
        public void InsertExplicitAccessModifiersOnStructs(IEnumerable<CodeItemStruct> structs)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs) return;

            foreach (var item in structs)
            {
                if (item?.CodeStruct == null) continue;
                var codeStruct = item.CodeStruct;

                var structDeclaration = CodeElementHelper.GetStructDeclaration(codeStruct);

                if (!IsAccessModifierExplicitlySpecifiedOnCodeElement(structDeclaration, codeStruct.Access))
                {
                    // Set the access value to itself to cause the code to be added.
                    try
                    {
                        codeStruct.Access = codeStruct.Access;
                    }
                    catch (Exception ex)
                    {
                        OutputWindowHelper.WarningWriteLine($"Unable to set explicit access modifier on struct {codeStruct.Name}: {ex.Message}");
                    }
                }
            }
        }

        #endregion Insertion Methods

        #region Helper Methods

        /// <summary>
        /// Determines if the access modifier is explicitly defined on the specified code element declaration.
        /// </summary>
        /// <param name="codeElementDeclaration">The code element declaration.</param>
        /// <param name="accessModifier">The access modifier.</param>
        /// <returns>True if access modifier is explicitly specified, otherwise false.</returns>
        private static bool IsAccessModifierExplicitlySpecifiedOnCodeElement(string codeElementDeclaration, vsCMAccess accessModifier)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string keyword = CodeElementHelper.GetAccessModifierKeyword(accessModifier);

            return IsKeywordSpecified(codeElementDeclaration, keyword);
        }

        /// <summary>
        /// Determines if the specified keyword is present in the specified code element declaration.
        /// </summary>
        /// <param name="codeElementDeclaration">The code element declaration.</param>
        /// <param name="keyword">The keyword.</param>
        /// <returns>True if the keyword is present, otherwise false.</returns>
        private static bool IsKeywordSpecified(string codeElementDeclaration, string keyword)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string matchString = @"(^|\s)" + keyword + @"\s";

            return RegexNullSafe.IsMatch(codeElementDeclaration, matchString);
        }

        #endregion Helper Methods
    }
}
