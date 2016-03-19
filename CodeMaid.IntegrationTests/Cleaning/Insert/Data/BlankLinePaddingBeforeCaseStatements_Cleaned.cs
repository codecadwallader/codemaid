using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBeforeCaseStatements
    {
        public void VoidReturnTest()
        {
            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;

                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    return;

                case DayOfWeek.Thursday: break;
                case DayOfWeek.Friday: return;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return;

                default:
                    return;
            }
        }

        public int IntReturnTest()
        {
            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;

                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    return
                        0 + 1;
                case DayOfWeek.Thursday: break;
                case DayOfWeek.Friday: return 2;
                case DayOfWeek.Saturday:
                    return 3;

                default:
                    return 4;
            }

            return 0;
        }
    }
}