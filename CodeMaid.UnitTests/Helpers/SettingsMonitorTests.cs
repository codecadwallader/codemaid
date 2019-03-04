using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    [TestClass]
    public class SettingsMonitorTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        public async Task CallbackShouldBeCalledAtOnce()
        {
            var monitor = new SettingsMonitor<Settings>(Settings.Default, null);

            int callbackTimes = 0;
            await monitor.WatchAsync(s => s.Feature_CleanupAllCode, _ =>
            {
                callbackTimes++;

                return Task.CompletedTask;
            });

            Assert.AreEqual(/*Initial Call Times*/1, callbackTimes);
        }

        [TestMethod]
        public async Task CallbackShouldNotBeCalledIfSettingNotChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            var monitor = new SettingsMonitor<Settings>(Settings.Default, null);

            bool? value = null;
            int callbackTimes = 0;
            await monitor.WatchAsync(s => s.Feature_CleanupAllCode, v =>
            {
                value = v;
                callbackTimes++;

                return Task.CompletedTask;
            });

            Settings.Default.Feature_CleanupAllCode = false;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/1 + 0, callbackTimes);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value);
        }

        [TestMethod]
        public async Task CallbackShouldBeCalledOnceSettingChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            var monitor = new SettingsMonitor<Settings>(Settings.Default, null);

            bool? value = null;
            int callbackTimes = 0;
            await monitor.WatchAsync(s => s.Feature_CleanupAllCode, v =>
            {
                value = v;
                callbackTimes++;

                return Task.CompletedTask;
            });

            Settings.Default.Feature_CleanupAllCode = true;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/ 1 + 1, callbackTimes);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value);
        }

        [TestMethod]
        public async Task AllCallbacksShouldBeCalledOnceSettingChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            var monitor = new SettingsMonitor<Settings>(Settings.Default, null);

            bool? value1 = null, value2 = null;
            int callbackTimes1 = 0, callbackTimes2 = 0;
            await monitor.WatchAsync(s => s.Feature_CleanupAllCode, v =>
            {
                value1 = v;
                callbackTimes1++;

                return Task.CompletedTask;
            });
            await monitor.WatchAsync(s => s.Feature_CleanupAllCode, v =>
            {
                value2 = v;
                callbackTimes2++;

                return Task.CompletedTask;
            });

            Settings.Default.Feature_CleanupAllCode = true;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/1 + 1, callbackTimes1);
            Assert.AreEqual(/*Initial Call Times*/1 + 1, callbackTimes2);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value1);
            Assert.AreEqual(Settings.Default.Feature_CleanupAllCode, value2);
        }

        [TestMethod]
        public async Task CallbackShouldBeCalledOnceAnyWatchedSettingChanged()
        {
            Settings.Default.Feature_CleanupAllCode = false;
            Settings.Default.Feature_CleanupOpenCode = false;
            Settings.Default.Feature_CleanupSelectedCode = true;
            var monitor = new SettingsMonitor<Settings>(Settings.Default, null);

            bool[] values = null;
            int callbackTimes = 0;
            await monitor.WatchAsync<bool>(new[]{
                nameof(Settings.Default.Feature_CleanupAllCode),
                nameof(Settings.Default.Feature_CleanupOpenCode),
                nameof(Settings.Default.Feature_CleanupSelectedCode)
            }, v =>
            {
                values = v;
                callbackTimes++;

                return Task.CompletedTask;
            });

            Settings.Default.Feature_CleanupSelectedCode = false;
            Settings.Default.Save();

            Assert.AreEqual(/*Initial Call Times*/1 + 1, callbackTimes);
            Assert.IsTrue(values.All(v => v == false));
        }
    }
}