using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SteveCadwallader.CodeMaid.UnitTests;

[TestClass]
public class RoslynTests
{
    public class EmtpyStatementRemoval : CSharpSyntaxRewriter
    {
        public override SyntaxNode Visit(SyntaxNode node)
        {
            return base.Visit(node);
        }

        public override SyntaxNode VisitEmptyStatement(EmptyStatementSyntax node)
        {
            //Construct an EmptyStatementSyntax with a missing semicolon
            return node.WithSemicolonToken(
                SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)
                    .WithLeadingTrivia(node.SemicolonToken.LeadingTrivia)
                    .WithTrailingTrivia(node.SemicolonToken.TrailingTrivia));
        }
    }

    [TestMethod]
    public void RunRewriter()
    {
        var tree = CSharpSyntaxTree.ParseText(@"
        public class Sample
        {
           public void Foo()
           {
              Console.WriteLine();

              #region SomeRegion

              //Some other code

              #endregion SomeRegion

              ;
            }
        }");

        var rewriter = new EmtpyStatementRemoval();
        var result = rewriter.Visit(tree.GetRoot());
        Console.WriteLine(result.ToFullString());
        Assert.AreEqual(1, 1);
    }
}