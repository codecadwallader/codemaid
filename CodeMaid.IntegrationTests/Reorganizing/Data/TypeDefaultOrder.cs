using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class TypeDefaultOrder
    {
        public const int PublicConstant = 1;

        public int PublicField;

        protected delegate void ProtectedDelegate();

        private const int PrivateConstant = 3;

        public interface PublicInterface
        {
        }

        private int PrivateField;

        public TypeDefaultOrder()
        {
        }

        internal event EventHandler InternalEvent;

        internal TypeDefaultOrder(bool param)
        {
        }

        internal const int InternalConstant = 1;

        internal delegate void InternalDelegate();

        protected TypeDefaultOrder(int param)
        {
        }

        protected interface ProtectedInterface
        {
        }

        public delegate void PublicDelegate();

        private delegate void PrivateDelegate();

        public event EventHandler PublicEvent;

        internal int InternalField;

        private event EventHandler PrivateEvent;

        public enum PublicEnum
        {
            Item1,
            Item2
        }

        internal enum InternalEnum
        {
        }

        private enum PrivateEnum
        {
        }

        internal interface InternalInterface
        {
        }

        protected string ProtectedProperty { set; }

        private interface PrivateInterface
        {
        }

        public static string PublicStaticProperty { get; set; }

        protected int ProtectedField;

        protected static void ProtectedStaticMethod()
        {
        }

        internal void InternalOverloadedMethod()
        {
        }

        public string PublicProperty { get; set; }

        public ~TypeDefaultOrder()
        {
        }

        private void PrivateOverloadedMethod(string input)
        {
        }

        internal string InternalProperty { get; }

        public void PublicMethod()
        {
        }

        protected static string ProtectedStaticProperty { set; }

        private TypeDefaultOrder(string param)
        {
        }

        internal static void InternalStaticMethod()
        {
        }

        protected enum ProtectedEnum
        {
        }

        protected void ProtectedOverloadedMethod()
        {
        }

        protected event EventHandler ProtectedEvent;

        private static string PrivateStaticProperty { get; set; }

        protected class ProtectedClass
        {
        }

        private string PrivateProperty { get; set; }

        public static void PublicStaticMethod()
        {
        }

        public struct PublicStruct
        {
        }

        static TypeDefaultOrder()
        {
        }

        public void PublicOverloadedMethod()
        {
        }

        protected void ProtectedMethod()
        {
        }

        public void PublicOverloadedMethod(string input)
        {
        }

        internal void InternalMethod()
        {
        }

        internal static string InternalStaticProperty { get; }

        protected void ProtectedOverloadedMethod(string input)
        {
        }

        protected struct ProtectedStruct
        {
        }

        public class PublicClass
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

        internal struct InternalStruct
        {
        }

        internal void InternalOverloadedMethod(string input)
        {
        }

        private struct PrivateStruct
        {
        }

        protected const int ProtectedConstant = 2;

        internal class InternalClass
        {
        }

        private class PrivateClass
        {
        }
    }
}