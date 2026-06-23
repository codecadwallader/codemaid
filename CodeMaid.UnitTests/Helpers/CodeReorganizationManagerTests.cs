using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NSubstitute;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    public class CodeReorganizationManagerTests
    {
        private readonly CodeReorganizationManager _manager;

        public CodeReorganizationManagerTests()
        {
            Settings.Default.Reset();

            InitializeUIThread();

            // Use FormatterServices.GetUninitializedObject to create an uninitialized instance,
            // to avoid initializing other VS components when calling the constructor because Package is null.
            _manager = (CodeReorganizationManager)FormatterServices.GetUninitializedObject(typeof(CodeReorganizationManager));
        }

        [Fact]
        public void ShouldReorganizeChildren_WhenParentIsEnum_ReturnsFalse()
        {
            var parent = new CodeItemEnum();

            var result = InvokeShouldReorganizeChildren(parent);

            Assert.False(result);
        }

        [Fact]
        public void ShouldReorganizeChildren_WhenParentIsClassWithoutAttributes_ReturnsTrue()
        {
            var parent = new CodeItemClass();

            var result = InvokeShouldReorganizeChildren(parent);

            Assert.True(result);
        }

        [Fact]
        public void ShouldReorganizeChildren_WhenParentHasComImportAttribute_ReturnsFalse()
        {
            var parent = new CodeItemClass();
            SetMockAttributes(parent, new[] { "System.Runtime.InteropServices.ComImportAttribute" });

            var result = InvokeShouldReorganizeChildren(parent);

            Assert.False(result);
        }

        [Fact]
        public void ShouldReorganizeChildren_WhenParentHasStructLayoutAttribute_ReturnsFalse()
        {
            var parent = new CodeItemClass();
            SetMockAttributes(parent, new[] { "System.Runtime.InteropServices.StructLayoutAttribute" });

            var result = InvokeShouldReorganizeChildren(parent);

            Assert.False(result);
        }

        [Fact]
        public void ShouldReorganizeChildren_WhenParentHasOtherAttributes_ReturnsTrue()
        {
            var parent = new CodeItemClass();
            SetMockAttributes(parent, new[] { "System.SerializableAttribute" });

            var result = InvokeShouldReorganizeChildren(parent);

            Assert.True(result);
        }

        [Fact]
        public void RegionsFlatten_WhenKeepMembersWithinRegionsIsTrue_ReturnsOriginalList()
        {
            Settings.Default.Reorganizing_KeepMembersWithinRegions = true;

            var field1 = new CodeItemField { Name = "Field1" };
            var subRegion = new CodeItemRegion { Name = "SubRegion" };
            var codeItems = new List<BaseCodeItem> { field1, subRegion };

            var result = InvokeRegionsFlatten(codeItems).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(field1, result);
            Assert.Contains(subRegion, result);
        }

        [Fact]
        public void RegionsFlatten_WhenKeepMembersWithinRegionsIsFalse_FlattensAllRegions()
        {
            Settings.Default.Reorganizing_KeepMembersWithinRegions = false;

            var field1 = new CodeItemField { Name = "Field1" };
            var field2 = new CodeItemField { Name = "Field2" };
            var method1 = new CodeItemMethod { Name = "Method1" };

            var subRegion = new CodeItemRegion { Name = "SubRegion" };
            subRegion.Children.Add(method1);

            var mainRegion = new CodeItemRegion { Name = "MainRegion" };
            mainRegion.Children.Add(field2);
            mainRegion.Children.Add(subRegion);

            var codeItems = new List<BaseCodeItem> { field1, mainRegion };

            var result = InvokeRegionsFlatten(codeItems).ToList();

            // 預期僅包含 field1, field2, method1，不應包含 any Region
            Assert.Equal(3, result.Count);
            Assert.Contains(field1, result);
            Assert.Contains(field2, result);
            Assert.Contains(method1, result);
            Assert.All(result, item => Assert.False(IsRegion(item)));
        }

        private static bool IsRegion(BaseCodeItem item)
        {
            return item is CodeItemRegion;
        }

        private static void InitializeUIThread()
        {
            // 強迫 ThreadHelper.Generic 被 lazy-init 初始化，使其內部實例不為 null
            var genericValue = ThreadHelper.Generic;

            // 將當前執行測試方法的執行緒註冊為 UI 執行緒
#pragma warning disable VSSDK005
            var context = new JoinableTaskContext(System.Threading.Thread.CurrentThread);
#pragma warning restore VSSDK005

            // 取得當前測試執行緒的 Dispatcher
            var dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

            // 1. 強制透過反射設定 ThreadHelper 的私有 static JoinableTaskContext 與 uiThreadDispatcher 欄位
            var fields = typeof(ThreadHelper).GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in fields)
            {
                if (f.FieldType == typeof(JoinableTaskContext))
                {
                    f.SetValue(null, context);
                }
                else if (f.FieldType == typeof(System.Windows.Threading.Dispatcher))
                {
                    f.SetValue(null, dispatcher);
                }
            }

            // 2. 設定 _generic 實例的所有 JoinableTaskContext 與 uiThreadDispatcher 欄位
            if (genericValue != null)
            {
                var instFields = typeof(ThreadHelper).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var f in instFields)
                {
                    if (f.FieldType == typeof(JoinableTaskContext))
                    {
                        f.SetValue(genericValue, context);
                    }
                    else if (f.FieldType == typeof(System.Windows.Threading.Dispatcher))
                    {
                        f.SetValue(genericValue, dispatcher);
                    }
                }
            }
        }

        private bool InvokeShouldReorganizeChildren(BaseCodeItemElement parent)
        {
            InitializeUIThread();

            var method = typeof(CodeReorganizationManager).GetMethod("ShouldReorganizeChildren",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                throw new InvalidOperationException("Could not find private method ShouldReorganizeChildren");
            }
            return (bool)method.Invoke(_manager, new object[] { parent });
        }

        private IEnumerable<BaseCodeItem> InvokeRegionsFlatten(IEnumerable<BaseCodeItem> codeItems)
        {
            var method = typeof(CodeReorganizationManager).GetMethod("RegionsFlatten",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                throw new InvalidOperationException("Could not find private method RegionsFlatten");
            }
            return (IEnumerable<BaseCodeItem>)method.Invoke(_manager, new object[] { codeItems });
        }

        private void SetMockAttributes(BaseCodeItemElement element, string[] attributeFullNames)
        {
            var mockAttributes = Substitute.For<CodeElements>();
            var attributesList = new List<CodeAttribute>();

            foreach (var fullName in attributeFullNames)
            {
                var mockAttr = Substitute.For<CodeAttribute>();
                mockAttr.FullName.Returns(fullName);
                attributesList.Add(mockAttr);
            }

            mockAttributes.GetEnumerator().Returns(attributesList.GetEnumerator());
            ((System.Collections.IEnumerable)mockAttributes).GetEnumerator().Returns(attributesList.GetEnumerator());

            var field = typeof(BaseCodeItemElement).GetField("_Attributes",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new InvalidOperationException("Could not find field _Attributes in BaseCodeItemElement");
            }
            field.SetValue(element, new Lazy<CodeElements>(() => mockAttributes));
        }
    }
}
