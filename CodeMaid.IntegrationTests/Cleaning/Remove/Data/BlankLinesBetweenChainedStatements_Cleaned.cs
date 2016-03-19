using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove.Data
{
    public class BlankLinesBetweenChainedStatements
    {
        public void Method()
        {
            if (true)
            {
            }
            else
            {
            }

            if (false)
            {
            }
            else if (true)
            {
            }
            else
            {
                
            }

            try
            {
            }
            catch (Exception)
            {
            }

            try
            {

            }
            catch (Exception)
            {
                
            }
            finally
            {
                
            }

            if (true)
            {

            }
            // Separating comment, should be ignored
            else
            {
                
            }

            try
            {

            }
                // Comment explaining the catch statement, should be ignored
            catch (Exception)
            {
                
                throw;
            }
        }
    }


}