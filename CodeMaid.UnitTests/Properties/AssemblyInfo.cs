using System.Reflection;
using Xunit;

[assembly: AssemblyTitle("SteveCadwallader.CodeMaid.UnitTests")]

// 停用測試並行化：各測試類別共享全域 Settings.Default 靜態狀態，
// 序列執行以避免競態（等同原先 MSTest 的預設行為）。
[assembly: CollectionBehavior(DisableTestParallelization = true)]