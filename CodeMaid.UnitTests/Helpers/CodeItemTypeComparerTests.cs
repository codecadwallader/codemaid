using EnvDTE80;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    [TestClass]
    public class CodeItemTypeComparerTests
    {
        [TestMethod]
        public void ShouldSortItemsOfTheSameTypeByName()
        {
            BaseCodeItem itemB = Create<CodeItemField>("b", 1);
            BaseCodeItem itemA = Create<CodeItemField>("a", 2);
            var comparer = new CodeItemTypeComparer(sortByName: true);

            int result = comparer.Compare(itemA, itemB);

            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void ShouldSortItemsOfTheSameTypeByOffset()
        {
            BaseCodeItem itemB = Create<CodeItemField>("b", 1);
            BaseCodeItem itemA = Create<CodeItemField>("a", 2);
            var comparer = new CodeItemTypeComparer(sortByName: false);

            int result = comparer.Compare(itemA, itemB);

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void ShouldSortByGroupType()
        {
            BaseCodeItem method = Create<CodeItemMethod>("a", 1);
            BaseCodeItem field = Create<CodeItemField>("z", 2);
            var comparer = new CodeItemTypeComparer(sortByName: true);

            int result = comparer.Compare(field, method);

            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void ShouldSortByExplicitInterfaceMemberName()
        {
            CodeItemMethod methodB = CreateExplicitMethod("Interface", "Z", 1);
            BaseCodeItem methodA = Create<CodeItemMethod>("X", 2);
            var comparer = new CodeItemTypeComparer(sortByName: true);

            int result = comparer.Compare(methodA, methodB);

            Assert.IsTrue(result < 0);
        }

        private static T Create<T>(string name, int offset) where T : BaseCodeItem, new()
        {
            return new T
            {
                Name = name,
                StartOffset = offset
            };
        }

        private static CodeItemMethod CreateExplicitMethod(string interfaceName, string methodName, int offset)
        {
            CodeItemMethod method = Create<CodeItemMethod>(interfaceName + "." + methodName, offset);
            method.CodeFunction = Substitute.For<CodeFunction2>();
            method.CodeFunction.Name = method.Name;
            return method;
        }
    }
}
