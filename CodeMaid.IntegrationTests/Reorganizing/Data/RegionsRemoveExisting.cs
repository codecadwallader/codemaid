using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class RegionsRemoveExisting
    {
        #region Arbitrary One Item Region

        public const int PublicConstant = 1;

        #endregion Arbitrary One Item Region

        public int PublicField;

        #region Arbitrary Multiple Item Region

        internal const int InternalConstant = 1;

        internal int InternalField;

        private const int PrivateConstant = 3;

        private int PrivateField;

        #endregion Arbitrary Multiple Item Region

        #region Constructors

        public RegionsRemoveExisting()
        {
            #region RegionsInMethodsShouldBeIgnored

            #endregion RegionsInMethodsShouldBeIgnored
        }

        protected RegionsRemoveExisting(int param)
        {
        }

        #endregion Constructors

        #region Arbitrary Nested Parent Region

        public delegate void PublicDelegate();

        internal delegate void InternalDelegate();

        protected delegate void ProtectedDelegate();

        private delegate void PrivateDelegate();

        #region Arbitrary Nested Child Region

        public event EventHandler PublicEvent;

        internal event EventHandler InternalEvent;

        internal event EventHandler InternalEvent2;

        private event EventHandler PrivateEvent;

        #endregion Arbitrary Nested Child Region

        public enum PublicEnum
        {
            Item1,
            Item2
        }

        #endregion Arbitrary Nested Parent Region

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