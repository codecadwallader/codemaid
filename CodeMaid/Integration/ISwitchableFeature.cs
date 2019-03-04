using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration
{
    internal interface ISwitchableFeature
    {
        Task SwitchAsync(bool on);
    }
}