using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class RegionsRemoveExisting
    {

        public const int PublicConstant = 1;

        public int PublicField;

        internal const int InternalConstant = 1;

        internal int InternalField;

        private const int PrivateConstant = 3;

        private int PrivateField;

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

        public delegate void PublicDelegate();

        internal delegate void InternalDelegate();

        protected delegate void ProtectedDelegate();

        private delegate void PrivateDelegate();

        public event EventHandler PublicEvent;

        internal event EventHandler InternalEvent;

        internal event EventHandler InternalEvent2;

        private event EventHandler PrivateEvent;

        public enum PublicEnum
        {
            Item1,
            Item2
        }

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