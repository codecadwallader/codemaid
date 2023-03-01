using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.UnitTests.Cleanup
{
    [TestClass]
    public class RoslynTests
    {
        private readonly TestWorkspace testWorkspace;

        public RoslynTests()
        {
            testWorkspace = new TestWorkspace();
        }

        [TestMethod]
        public async Task ShouldAddClassAccessorAsync()
        {
            var source =
    """
class MyClass
{
}
""";

            var expected =
    """
internal class MyClass
{
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddSamePartialClassAccessorsAsync()
        {
            var source =
    """
public partial class Temp
{
}

partial class Temp
{
}
""";

            var expected =
    """
public partial class Temp
{
}

public partial class Temp
{
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddNestedClassAccessorAsync()
        {
            var source =
    """
class Temp
{
    int MyProperty { get; set; }
}

public class Outer
{
    class Temp
    {
        int MyProperty { get; set; }
    }
}
""";

            var expected =
    """
internal class Temp
{
    private int MyProperty { get; set; }
}

public class Outer
{
    private class Temp
    {
        private int MyProperty { get; set; }
    }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddStructAccessorAsync()
        {
            var source =
    """
struct MyStruct
{
}
""";

            var expected =
    """
internal struct MyStruct
{
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddRefStructAccessorAsync()
        {
            var source =
    """
ref struct MyStruct
{
}

readonly ref struct MyReadonlyStruct
{
}
""";

            var expected =
    """
internal ref struct MyStruct
{
}

internal readonly ref struct MyReadonlyStruct
{
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddPropertyAccessorAsync()
        {
            var source =
    """
class Sample
{
    int Prop { get; set; }
}
""";

            var expected =
    """
internal class Sample
{
    private int Prop { get; set; }
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldNotRemoveRequiredPropertyAsync()
        {
            var source =
    """
class Sample
{
    required int Prop { get; set; }
}
""";

            var expected =
    """
internal class Sample
{
    private required int Prop { get; set; }
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddMethodsAccessorAsync()
        {
            var source =
    """
class ExampleClass
{
    void Do()
    {
    }
}
""";

            var expected =
    """
internal class ExampleClass
{
    private void Do()
    {
    }
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldNotAddPartialMethodAccessorAsync()
        {
            var source =
    """
public partial class ExampleClass
{
    partial void Do()
    {
    }
}
""";

            var expected =
    """
public partial class ExampleClass
{
    partial void Do()
    {
    }
}
""";

            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddDefaultAbstractVirtualAccessorsAsync()
        {
            var source =
    """
abstract class MyAbstract
{
    virtual void VirtualMethod()
    {
    }

    abstract void AbstractMethod();
}
""";

            var expected =
    """
internal abstract class MyAbstract
{
    public virtual void VirtualMethod()
    {
    }

    protected abstract void AbstractMethod();
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task TestInheritsAbstractAsync()
        {
            var source =
    """
abstract class MyAbstract
{
    private protected abstract void AbstractMethod();
}

class Derive : MyAbstract
{
    override void AbstractMethod()
    {
    }
}
""";

            var expected =
    """
internal abstract class MyAbstract
{
    private protected abstract void AbstractMethod();
}

internal class Derive : MyAbstract
{
    private protected override void AbstractMethod()
    {
    }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task TestShouldNotRemoveFileAsync()
        {
            var source =
    """
file class MyFile
{
    int Prop { get; set; }
}

file struct MyFileStruct
{
    int Prop { get; set; }
}
""";

            var expected =
    """
file class MyFile
{
    private int Prop { get; set; }
}

file struct MyFileStruct
{
    private int Prop { get; set; }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddDelegateAccessorAsync()
        {
            var source =
    """
class MyDelegate
{
    delegate int PerformCalculation(int x, int y);
}
""";

            var expected =
    """
internal class MyDelegate
{
    private delegate int PerformCalculation(int x, int y);
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddEventAccessorAsync()
        {
            var source =
    """
class MyEvent
{
    event MyEventHandler MyEvent;
}
""";

            var expected =
    """
internal class MyEvent
{
    private event MyEventHandler MyEvent;
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddEnumAccessorAsync()
        {
            var source =
    """
enum MyEnum
{
    Some = 0,
    None = 1,
}
""";

            var expected =
    """
internal enum MyEnum
{
    Some = 0,
    None = 1,
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddNestedEnumAccessorAsync()
        {
            var source =
    """
class MyClass
{
    enum MyEnum
    {
        Some = 0,
        None = 1,
    }
}
""";

            var expected =
    """
internal class MyClass
{
    private enum MyEnum
    {
        Some = 0,
        None = 1,
    }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddInterfaceAccessorAsync()
        {
            var source =
    """
interface IMyInterface
{
    int MyProp { get; set; }

    void Do();
}
""";

            var expected =
    """
internal interface IMyInterface
{
    int MyProp { get; set; }

    void Do();
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddNestedInterfaceAccessorAsync()
        {
            var source =
    """
class MyClass
{
    interface IMyInterface
    {
        int MyProp { get; set; }

        void Do();
    }
}
""";

            var expected =
    """
internal class MyClass
{
    private interface IMyInterface
    {
        int MyProp { get; set; }

        void Do();
    }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldAddFieldAccessorAsync()
        {
            var source =
    """
class MyClass
{
    int _number;
}
""";

            var expected =
    """
internal class MyClass
{
    private int _number;
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldNotChangeInvalidSyntaxAsync()
        {
            var source =
    """
class ITemp
{
    void Do()
    {
        int MyProperty { get; set; }
    }
}
""";

            var expected =
    """
internal class ITemp
{
    private void Do()
    {
        int MyProperty { get; set; }
    }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }

        [TestMethod]
        public async Task ShouldNotChangeInterfaceDescendantsAsync()
        {
            var source =
    """
interface IInterface
{
    class C
    {
        class D
        {

        }
    }
}
""";

            var expected =
    """
internal interface IInterface
{
    class C
    {
        class D
        {

        }
    }
}
""";
            await testWorkspace.VerifyCleanupAsync(source, expected);
        }
    }
}