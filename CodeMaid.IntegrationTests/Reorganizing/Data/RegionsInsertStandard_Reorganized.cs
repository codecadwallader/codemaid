using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class RegionsInsertStandard
    {
        #region Fields

        public const int PublicConstant = 1;

        public int PublicField;

        internal const int InternalConstant = 1;

        internal int InternalField;

        private const int PrivateConstant = 3;

        private int PrivateField;

        #endregion Fields

        #region Constructors

        public RegionsInsertStandard()
        {
        }

        protected RegionsInsertStandard(int param)
        {
        }

        #endregion Constructors

        #region Delegates

        public delegate void PublicDelegate();

        internal delegate void InternalDelegate();

        protected delegate void ProtectedDelegate();

        private delegate void PrivateDelegate();

        #endregion Delegates

        #region Events

        public event EventHandler PublicEvent;

        internal event EventHandler InternalEvent;

        internal event EventHandler InternalEvent2;

        private event EventHandler PrivateEvent;

        #endregion Events

        #region Enums

        public enum PublicEnum
        {
            Item1,
            Item2
        }

        #endregion Enums

        #region Interfaces

        public interface PublicInterface
        {
        }

        protected interface ProtectedInterface
        {
        }

        #endregion Interfaces
    }
}