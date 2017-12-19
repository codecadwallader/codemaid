using SteveCadwallader.CodeMaid.Integration.Commands;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace SteveCadwallader.CodeMaid
{
    internal class StringResourceKey : Properties.Resources
    {
        private static StringDictionary sdCommandTexts = null;

        internal static string GetResourceText(BaseCommand command)
        {
            if (sdCommandTexts == null)
            {
                sdCommandTexts = new StringDictionary();
                var resquery = from p in typeof(PackageGuids).GetFields()
                               where p.FieldType == typeof(Guid) && !string.IsNullOrEmpty(ResourceManager.GetString(p.Name))
                               select new Tuple<string, string>(((Guid)p.GetValue(null)).ToString(), ResourceManager.GetString(p.Name));
                resquery.ToList().ForEach(p => sdCommandTexts.Add(p.Item1, p.Item2));
            }
            string endstring = command.Text;
            if (sdCommandTexts.ContainsKey(command.CommandID.Guid.ToString()))
            {
                endstring = sdCommandTexts[command.CommandID.Guid.ToString()];
            }
            else
            {
                endstring = command.Text;
            }
            return endstring;
        }
    }
}