using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class ReorganizationAvailability
    {
        private static void Main(string[] args)
        {
#if DEBUG
            Debug();
#endif
            Always();
        }

#if DEBUG

        private static void Debug()
        {
            Console.WriteLine("BUGS");
        }

#endif

        public static void Always()
        {
            Console.WriteLine("Defect free code");
        }
    }
}