using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class TypeCustomOrder
    {
        static TypeCustomOrder()
        {
        }

        public TypeCustomOrder()
        {
        }

        internal TypeCustomOrder(bool param)
        {
        }

        protected TypeCustomOrder(int param)
        {
        }

        private TypeCustomOrder(string param)
        {
        }

        public delegate void PublicDelegate();

        internal delegate void InternalDelegate();

        protected delegate void ProtectedDelegate();

        private delegate void PrivateDelegate();

        public event EventHandler PublicEvent;

        internal event EventHandler InternalEvent;

        protected event EventHandler ProtectedEvent;

        private event EventHandler PrivateEvent;

        public enum PublicEnum
        {
            Item1,
            Item2
        }

        internal enum InternalEnum
        {
        }

        protected enum ProtectedEnum
        {
        }

        private enum PrivateEnum
        {
        }

        public interface PublicInterface
        {
        }

        internal interface InternalInterface
        {
        }

        protected interface ProtectedInterface
        {
        }

        private interface PrivateInterface
        {
        }

        public static string PublicStaticProperty { get; set; }
        public string PublicProperty { get; set; }
        internal static string InternalStaticProperty { get; }
        internal string InternalProperty { get; }
        protected static string ProtectedStaticProperty { set; }
        protected string ProtectedProperty { set; }
        private static string PrivateStaticProperty { get; set; }
        private string PrivateProperty { get; set; }
        public static void PublicStaticMethod()
        {
        }

        public void PublicMethod()
        {
        }

        public void PublicOverloadedMethod()
        {
        }

        public void PublicOverloadedMethod(string input)
        {
        }

        internal static void InternalStaticMethod()
        {
        }

        internal void InternalMethod()
        {
        }

        internal void InternalOverloadedMethod()
        {
        }

        internal void InternalOverloadedMethod(string input)
        {
        }

        protected static void ProtectedStaticMethod()
        {
        }

        protected void ProtectedMethod()
        {
        }

        protected void ProtectedOverloadedMethod()
        {
        }

        protected void ProtectedOverloadedMethod(string input)
        {
        }

        private static void PrivateStaticMethod()
        {
        }

        private void PrivateMethod()
        {
        }

        private void PrivateOverloadedMethod()
        {
        }

        private void PrivateOverloadedMethod(string input)
        {
        }

        public struct PublicStruct
        {
        }

        internal struct InternalStruct
        {
        }

        protected struct ProtectedStruct
        {
        }

        private struct PrivateStruct
        {
        }

        public class PublicClass
        {
        }

        internal class InternalClass
        {
        }

        protected class ProtectedClass
        {
        }

        private class PrivateClass
        {
        }

        public const int PublicConstant = 1;

        public int PublicField;

        internal const int InternalConstant = 1;

        internal int InternalField;

        protected const int ProtectedConstant = 2;

        protected int ProtectedField;

        private const int PrivateConstant = 3;

        private int PrivateField;
        public ~TypeCustomOrder()
        {
        }

        internal ~TypeCustomOrder()
        {
        }

        protected ~TypeCustomOrder()
        {
        }

        private ~TypeCustomOrder()
        {
        }
    }
}
