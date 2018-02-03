using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    [TestClass]
    public class SettingMonitorTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        public void CallbackShouldBeCalledAtOnce()
        {
            var monitor = new SettingMonitor<Settings>(Settings.Default);

            int callbackTimes = 0;
            monitor.Watch(s => s.Feature_CleanupAllCode, _ => callbackTimes++);

            Assert.AreEqual(/*Initial Call Times*/1, callbackTimes);
        }

        [TestMethod]
        public void CallbackShouldNotBeCalledIfSettingNotChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            var monitor = new SettingMonitor<Settings>(Settings.Default);

            bool? value = null;
            int callbackTimes = 0;
            monitor.Watch(s => s.Feature_CleanupAllCode, v =>
            {
                value = v;
                callbackTimes++;
            });

            Settings.Default.Feature_CleanupAllCode = false;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/1 + 0, callbackTimes);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value);
        }

        [TestMethod]
        public void CallbackShouldBeCalledOnceSettingChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            var monitor = new SettingMonitor<Settings>(Settings.Default);

            bool? value = null;
            int callbackTimes = 0;
            monitor.Watch(s => s.Feature_CleanupAllCode, v =>
            {
                value = v;
                callbackTimes++;
            });

            Settings.Default.Feature_CleanupAllCode = true;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/ 1 + 1, callbackTimes);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value);
        }

        [TestMethod]
        public void AllCallbacksShouldBeCalledOnceSettingChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            var monitor = new SettingMonitor<Settings>(Settings.Default);

            bool? value1 = null, value2 = null;
            int callbackTimes1 = 0, callbackTimes2 = 0;
            monitor.Watch(s => s.Feature_CleanupAllCode, v =>
            {
                value1 = v;
                callbackTimes1++;
            });
            monitor.Watch(s => s.Feature_CleanupAllCode, v =>
            {
                value2 = v;
                callbackTimes2++;
            });

            Settings.Default.Feature_CleanupAllCode = true;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/1 + 1, callbackTimes1);
            Assert.AreEqual(/*Initial Call Times*/1 + 1, callbackTimes2);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value1);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value2);
        }

        [TestMethod]
        public void CallbackShouldBeCalledOnceAnyWatchedSettingChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            Settings.Default.Feature_CleanupOpenCode = false;
            Settings.Default.Feature_CleanupSelectedCode = true;
            var monitor = new SettingMonitor<Settings>(Settings.Default);

            bool[] values = null;
            int callbackTimes = 0;
            monitor.Watch<bool>(new[]{
                nameof(Settings.Default.Feature_CleanupAllCode),
                nameof(Settings.Default.Feature_CleanupOpenCode),
                nameof(Settings.Default.Feature_CleanupSelectedCode)
            }, v =>
            {
                values = v;
                callbackTimes++;
            });

            Settings.Default.Feature_CleanupSelectedCode = false;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/1 + 1, callbackTimes);
            Assert.IsTrue(values.All(v => v == false));
        }
    }
}