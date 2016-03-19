namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update.Data
{
    public abstract class SingleLineMethods
    {
        public void Method()
        {
        }

        public void Method2()
        {
            /* With comment */
        }

        public void Method3()
        {
            if (true) { }
        }

        // Abstract methods are a single line, but do not have a body so should not be affected.
        public abstract void AbstractMethod();
    }

    public interface ISingleLineMethods
    {
        // Interface methods are a single line, but do not have a body so should not be affected.
        void Method();
    }
}