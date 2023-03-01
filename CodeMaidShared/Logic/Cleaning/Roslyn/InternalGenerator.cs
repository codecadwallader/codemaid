using CodeMaidShared.Logic.Cleaning;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    internal static class InternalGenerator
    {
        private static SyntaxTokenList GetModifierTokens(SyntaxNode declaration)
               => CSharpAccessibilityFacts.GetModifierTokens(declaration);

        private static void GetAccessibilityAndModifiers(SyntaxTokenList modifierList, out Accessibility accessibility, out DeclarationModifiers modifiers, out bool isDefault)
             => CSharpAccessibilityFacts.GetAccessibilityAndModifiers(modifierList, out accessibility, out modifiers, out isDefault);

        public static SyntaxNode WithAccessibility(SyntaxNode declaration, Accessibility accessibility)
        {
            if (!CSharpAccessibilityFacts.CanHaveAccessibility(declaration) &&
                accessibility != Accessibility.NotApplicable)
            {
                return declaration;
            }

            return Isolate(declaration, d =>
            {
                var tokens = GetModifierTokens(d);
                GetAccessibilityAndModifiers(tokens, out _, out var modifiers, out _);
                if (modifiers.IsFile && accessibility != Accessibility.NotApplicable)
                {
                    // If user wants to set accessibility for a file-local declaration, we remove file.
                    // Otherwise, code will be in error:
                    // error CS9052: File-local type '{0}' cannot use accessibility modifiers.
                    modifiers = modifiers.WithIsFile(false);
                }

                if (modifiers.IsStatic && declaration.IsKind(SyntaxKind.ConstructorDeclaration) && accessibility != Accessibility.NotApplicable)
                {
                    // If user wants to add accessibility for a static constructor, we remove static modifier
                    modifiers = modifiers.WithIsStatic(false);
                }

                var newTokens = Merge(tokens, AsModifierList(accessibility, modifiers));
                return SetModifierTokens(d, newTokens);
            });
        }

        private static SyntaxNode SetModifierTokens(SyntaxNode declaration, SyntaxTokenList modifiers)
               => declaration switch
               {
                   MemberDeclarationSyntax memberDecl => memberDecl.WithModifiers(modifiers),
                   ParameterSyntax parameter => parameter.WithModifiers(modifiers),
                   LocalDeclarationStatementSyntax localDecl => localDecl.WithModifiers(modifiers),
                   LocalFunctionStatementSyntax localFunc => localFunc.WithModifiers(modifiers),
                   AccessorDeclarationSyntax accessor => accessor.WithModifiers(modifiers),
                   AnonymousFunctionExpressionSyntax anonymous => anonymous.WithModifiers(modifiers),
                   _ => declaration,
               };

        internal static SyntaxTokenList Merge(SyntaxTokenList original, SyntaxTokenList newList)
        {
            // return tokens from newList, but use original tokens of kind matches
            return new SyntaxTokenList(newList.Select(
                token => Any(original, token.RawKind)
                    ? original.First(tk => tk.RawKind == token.RawKind)
                    : token));
        }

        private static bool Any(SyntaxTokenList original, int rawKind)
        {
            foreach (var token in original)
            {
                if (token.RawKind == rawKind)
                {
                    return true;
                }
            }

            return false;
        }

        private static SyntaxTokenList AsModifierList(Accessibility accessibility, DeclarationModifiers modifiers, SyntaxKind kind)
                => AsModifierList(accessibility, GetAllowedModifiers(kind) & modifiers);

        private static readonly DeclarationModifiers s_fieldModifiers =
        DeclarationModifiers.Const |
        DeclarationModifiers.New |
        DeclarationModifiers.ReadOnly |
        DeclarationModifiers.Required |
        DeclarationModifiers.Static |
        DeclarationModifiers.Unsafe |
        DeclarationModifiers.Volatile;

        private static readonly DeclarationModifiers s_methodModifiers =
            DeclarationModifiers.Abstract |
            DeclarationModifiers.Async |
            DeclarationModifiers.Extern |
            DeclarationModifiers.New |
            DeclarationModifiers.Override |
            DeclarationModifiers.Partial |
            DeclarationModifiers.ReadOnly |
            DeclarationModifiers.Sealed |
            DeclarationModifiers.Static |
            DeclarationModifiers.Virtual |
            DeclarationModifiers.Unsafe;

        private static readonly DeclarationModifiers s_constructorModifiers =
            DeclarationModifiers.Extern |
            DeclarationModifiers.Static |
            DeclarationModifiers.Unsafe;

        private static readonly DeclarationModifiers s_destructorModifiers = DeclarationModifiers.Unsafe;
        private static readonly DeclarationModifiers s_propertyModifiers =
            DeclarationModifiers.Abstract |
            DeclarationModifiers.Extern |
            DeclarationModifiers.New |
            DeclarationModifiers.Override |
            DeclarationModifiers.ReadOnly |
            DeclarationModifiers.Required |
            DeclarationModifiers.Sealed |
            DeclarationModifiers.Static |
            DeclarationModifiers.Virtual |
            DeclarationModifiers.Unsafe;

        private static readonly DeclarationModifiers s_eventModifiers =
            DeclarationModifiers.Abstract |
            DeclarationModifiers.Extern |
            DeclarationModifiers.New |
            DeclarationModifiers.Override |
            DeclarationModifiers.ReadOnly |
            DeclarationModifiers.Sealed |
            DeclarationModifiers.Static |
            DeclarationModifiers.Virtual |
            DeclarationModifiers.Unsafe;

        private static readonly DeclarationModifiers s_eventFieldModifiers =
            DeclarationModifiers.New |
            DeclarationModifiers.ReadOnly |
            DeclarationModifiers.Static |
            DeclarationModifiers.Unsafe;

        private static readonly DeclarationModifiers s_indexerModifiers =
            DeclarationModifiers.Abstract |
            DeclarationModifiers.Extern |
            DeclarationModifiers.New |
            DeclarationModifiers.Override |
            DeclarationModifiers.ReadOnly |
            DeclarationModifiers.Sealed |
            DeclarationModifiers.Static |
            DeclarationModifiers.Virtual |
            DeclarationModifiers.Unsafe;

        private static readonly DeclarationModifiers s_classModifiers =
            DeclarationModifiers.Abstract |
            DeclarationModifiers.New |
            DeclarationModifiers.Partial |
            DeclarationModifiers.Sealed |
            DeclarationModifiers.Static |
            DeclarationModifiers.Unsafe |
            DeclarationModifiers.File;

        private static readonly DeclarationModifiers s_recordModifiers =
            DeclarationModifiers.Abstract |
            DeclarationModifiers.New |
            DeclarationModifiers.Partial |
            DeclarationModifiers.Sealed |
            DeclarationModifiers.Unsafe |
            DeclarationModifiers.File;

        private static readonly DeclarationModifiers s_structModifiers =
            DeclarationModifiers.New |
            DeclarationModifiers.Partial |
            DeclarationModifiers.ReadOnly |
            DeclarationModifiers.Ref |
            DeclarationModifiers.Unsafe |
            DeclarationModifiers.File;

        private static readonly DeclarationModifiers s_interfaceModifiers = DeclarationModifiers.New | DeclarationModifiers.Partial | DeclarationModifiers.Unsafe | DeclarationModifiers.File;
        private static readonly DeclarationModifiers s_accessorModifiers = DeclarationModifiers.Abstract | DeclarationModifiers.New | DeclarationModifiers.Override | DeclarationModifiers.Virtual;

        private static readonly DeclarationModifiers s_localFunctionModifiers =
            DeclarationModifiers.Async |
            DeclarationModifiers.Static |
            DeclarationModifiers.Unsafe |
            DeclarationModifiers.Extern;

        private static readonly DeclarationModifiers s_lambdaModifiers =
            DeclarationModifiers.Async |
            DeclarationModifiers.Static;

        private static DeclarationModifiers GetAllowedModifiers(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.RecordDeclaration:
                    return s_recordModifiers;

                case SyntaxKind.ClassDeclaration:
                    return s_classModifiers;

                case SyntaxKind.EnumDeclaration:
                    return DeclarationModifiers.New | DeclarationModifiers.File;

                case SyntaxKind.DelegateDeclaration:
                    return DeclarationModifiers.New | DeclarationModifiers.Unsafe | DeclarationModifiers.File;

                case SyntaxKind.InterfaceDeclaration:
                    return s_interfaceModifiers;

                case SyntaxKind.StructDeclaration:
                case SyntaxKind.RecordStructDeclaration:
                    return s_structModifiers;

                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    return s_methodModifiers;

                case SyntaxKind.ConstructorDeclaration:
                    return s_constructorModifiers;

                case SyntaxKind.DestructorDeclaration:
                    return s_destructorModifiers;

                case SyntaxKind.FieldDeclaration:
                    return s_fieldModifiers;

                case SyntaxKind.PropertyDeclaration:
                    return s_propertyModifiers;

                case SyntaxKind.IndexerDeclaration:
                    return s_indexerModifiers;

                case SyntaxKind.EventFieldDeclaration:
                    return s_eventFieldModifiers;

                case SyntaxKind.EventDeclaration:
                    return s_eventModifiers;

                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                    return s_accessorModifiers;

                case SyntaxKind.LocalFunctionStatement:
                    return s_localFunctionModifiers;

                case SyntaxKind.ParenthesizedLambdaExpression:
                case SyntaxKind.SimpleLambdaExpression:
                case SyntaxKind.AnonymousMethodExpression:
                    return s_lambdaModifiers;

                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.Parameter:
                case SyntaxKind.LocalDeclarationStatement:
                default:
                    return DeclarationModifiers.None;
            }
        }

        private static SyntaxTokenList AsModifierList(Accessibility accessibility, DeclarationModifiers modifiers)
        {
            var list = new List<SyntaxToken>();

            switch (accessibility)
            {
                case Accessibility.Internal:
                    list.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                    break;

                case Accessibility.Public:
                    list.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    break;

                case Accessibility.Private:
                    list.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                    break;

                case Accessibility.Protected:
                    list.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                    break;

                case Accessibility.ProtectedOrInternal:
                    list.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                    list.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                    break;

                case Accessibility.ProtectedAndInternal:
                    list.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                    list.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                    break;

                case Accessibility.NotApplicable:
                    break;
            }

            if (modifiers.IsFile)
                list.Add(SyntaxFactory.Token(SyntaxKind.FileKeyword));

            if (modifiers.IsAbstract)
                list.Add(SyntaxFactory.Token(SyntaxKind.AbstractKeyword));

            if (modifiers.IsNew)
                list.Add(SyntaxFactory.Token(SyntaxKind.NewKeyword));

            if (modifiers.IsSealed)
                list.Add(SyntaxFactory.Token(SyntaxKind.SealedKeyword));

            if (modifiers.IsOverride)
                list.Add(SyntaxFactory.Token(SyntaxKind.OverrideKeyword));

            if (modifiers.IsVirtual)
                list.Add(SyntaxFactory.Token(SyntaxKind.VirtualKeyword));

            if (modifiers.IsStatic)
                list.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

            if (modifiers.IsAsync)
                list.Add(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));

            if (modifiers.IsConst)
                list.Add(SyntaxFactory.Token(SyntaxKind.ConstKeyword));

            if (modifiers.IsReadOnly)
                list.Add(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));

            if (modifiers.IsUnsafe)
                list.Add(SyntaxFactory.Token(SyntaxKind.UnsafeKeyword));

            if (modifiers.IsVolatile)
                list.Add(SyntaxFactory.Token(SyntaxKind.VolatileKeyword));

            if (modifiers.IsExtern)
                list.Add(SyntaxFactory.Token(SyntaxKind.ExternKeyword));

            if (modifiers.IsRequired)
                list.Add(SyntaxFactory.Token(SyntaxKind.RequiredKeyword));

            // partial and ref must be last
            if (modifiers.IsRef)
                list.Add(SyntaxFactory.Token(SyntaxKind.RefKeyword));

            if (modifiers.IsPartial)
                list.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            list = list.Select(x => x.WithTrailingTrivia(SyntaxFactory.Space)).ToList();

            return SyntaxFactory.TokenList(list);
        }

        private static SyntaxNode Isolate(SyntaxNode declaration, Func<SyntaxNode, SyntaxNode> editor)
              => PreserveTrivia(AsIsolatedDeclaration(declaration), editor);

        private static SyntaxNode AsIsolatedDeclaration(SyntaxNode declaration)
        {
            switch (declaration.Kind())
            {
                case SyntaxKind.VariableDeclaration:
                    var vd = (VariableDeclarationSyntax)declaration;
                    if (vd.Parent != null && vd.Variables.Count == 1)
                    {
                        return AsIsolatedDeclaration(vd.Parent);
                    }

                    break;

                case SyntaxKind.VariableDeclarator:
                    var v = (VariableDeclaratorSyntax)declaration;
                    if (v.Parent != null && v.Parent.Parent != null)
                    {
                        return ClearTrivia(WithVariable(v.Parent.Parent, v));
                    }

                    break;

                case SyntaxKind.Attribute:
                    var attr = (AttributeSyntax)declaration;
                    if (attr.Parent != null)
                    {
                        var attrList = (AttributeListSyntax)attr.Parent;
                        return attrList.WithAttributes(SyntaxFactory.SingletonSeparatedList(attr)).WithTarget(null);
                    }

                    break;
            }

            return declaration;
        }

        private static SyntaxNode WithVariable(SyntaxNode declaration, VariableDeclaratorSyntax variable)
        {
            var vd = GetVariableDeclaration(declaration);
            if (vd != null)
            {
                return WithVariableDeclaration(declaration, vd.WithVariables(SyntaxFactory.SingletonSeparatedList(variable)));
            }

            return declaration;
        }

        private static VariableDeclarationSyntax? GetVariableDeclaration(SyntaxNode declaration)
            => declaration.Kind() switch
            {
                SyntaxKind.FieldDeclaration => ((FieldDeclarationSyntax)declaration).Declaration,
                SyntaxKind.EventFieldDeclaration => ((EventFieldDeclarationSyntax)declaration).Declaration,
                SyntaxKind.LocalDeclarationStatement => ((LocalDeclarationStatementSyntax)declaration).Declaration,
                _ => null,
            };

        private static SyntaxNode WithVariableDeclaration(SyntaxNode declaration, VariableDeclarationSyntax variables)
            => declaration.Kind() switch
            {
                SyntaxKind.FieldDeclaration => ((FieldDeclarationSyntax)declaration).WithDeclaration(variables),
                SyntaxKind.EventFieldDeclaration => ((EventFieldDeclarationSyntax)declaration).WithDeclaration(variables),
                SyntaxKind.LocalDeclarationStatement => ((LocalDeclarationStatementSyntax)declaration).WithDeclaration(variables),
                _ => declaration,
            };

        public static TNode ClearTrivia<TNode>(TNode node) where TNode : SyntaxNode
        {
            if (node != null)
            {
                return node.WithLeadingTrivia(SyntaxFactory.ElasticMarker)
                           .WithTrailingTrivia(SyntaxFactory.ElasticMarker);
            }
            else
            {
                return null;
            }
        }

        private static SyntaxNode? PreserveTrivia<TNode>(TNode? node, Func<TNode, SyntaxNode> nodeChanger) where TNode : SyntaxNode
        {
            if (node == null)
            {
                return node;
            }

            var nodeWithoutTrivia = node.WithoutLeadingTrivia().WithoutTrailingTrivia();

            var changedNode = nodeChanger(nodeWithoutTrivia);
            if (changedNode == nodeWithoutTrivia)
            {
                return node;
            }

            return changedNode
                .WithLeadingTrivia(node.GetLeadingTrivia().Concat(changedNode.GetLeadingTrivia()))
                .WithTrailingTrivia(changedNode.GetTrailingTrivia().Concat(node.GetTrailingTrivia()));
        }
    }
}