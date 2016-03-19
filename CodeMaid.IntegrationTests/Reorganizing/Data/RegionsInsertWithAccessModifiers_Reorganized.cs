using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class RegionsInsertWithAccessModifiers
    {
        #region Public Fields

        public const int PublicConstant = 1;

        public int PublicField;

        #endregion Public Fields

        #region Internal Fields

        internal const int InternalConstant = 1;

        internal int InternalField;

        #endregion Internal Fields

        #region Private Fields

        private const int PrivateConstant = 3;

        private int PrivateField;

        #endregion Private Fields

        #region Constructors

        public RegionsInsertWithAccessModifiers()
        {
        }

        protected RegionsInsertWithAccessModifiers(int param)
        {
        }

        #endregion Constructors

        #region Public Delegates

        public delegate void PublicDelegate();

        #endregion Public Delegates

        #region Internal Delegates

        internal delegate void InternalDelegate();

        #endregion Internal Delegates

        #region Protected Delegates

        protected delegate void ProtectedDelegate();

        #endregion Protected Delegates

        #region Private Delegates

        private delegate void PrivateDelegate();

        #endregion Private Delegates

        #region Public Events

        public event EventHandler PublicEvent;

        #endregion Public Events

        #region Internal Events

        internal event EventHandler InternalEvent;

        internal event EventHandler InternalEvent2;

        #endregion Internal Events

        #region Private Events

        private event EventHandler PrivateEvent;

        #endregion Private Events

        #region Public Enums

        public enum PublicEnum
        {
            Item1,
            Item2
        }

        #endregion Public Enums

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