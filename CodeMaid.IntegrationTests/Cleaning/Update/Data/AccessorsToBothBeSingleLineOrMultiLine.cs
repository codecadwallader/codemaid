using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update.Data
{
    public class AccessorsToBothBeSingleLineOrMultiLine
    {
        private EventHandler _event;
        private bool _field;

        public event EventHandler SingleLineEvent
        {
            add { _event += value; }
            remove { _event -= value; }
        }

        public event EventHandler SingleLineAdderSingleLineBodyRemover
        {
            add { _event += value; }
            remove
            {
                _event -= value;
            }
        }

        public event EventHandler SingleLineAdderMultiLineBodyRemover
        {
            add { _event += value; }
            remove
            {
                if (_event != null)
                {
                    _event -= value;
                }
            }
        }

        public event EventHandler MultiLineAdderSingleLineRemover
        {
            add
            {
                if (_event == null)
                {
                    _event += value;
                }
            }
            remove { _event -= value; }
        }

        public event EventHandler MultiLineAdderSingleLineBodyRemover
        {
            add
            {
                if (_event == null)
                {
                    _event += value;
                }
            }
            remove
            {
                _event -= value;
            }
        }

        public event EventHandler MultiLineAdderMultiLineRemover
        {
            add
            {
                if (_event == null)
                {
                    _event += value;
                }
            }
            remove
            {
                if (_event != null)
                {
                    _event -= value;
                }
            }
        }

        public bool AutoImplementedProperty { get; set; }

        public bool SingleLineProperty
        {
            get { return _field; }
            set { _field = value; }
        }

        public bool SingleLineGetterSingleLineBodySetter
        {
            get { return _field; }
            set
            {
                _field = value;
            }
        }

        public bool SingleLineGetterMultiLineBodySetter
        {
            get { return _field; }
            set
            {
                if (_field != value)
                {
                    _field = value;
                }
            }
        }

        public bool MultiLineGetterSingleLineSetter
        {
            get
            {
                if (_field == false)
                {
                    _field = true;
                }

                return _field;
            }
            set { _field = value; }
        }

        public bool MultiLineGetterSingleLineBodySetter
        {
            get
            {
                if (_field == false)
                {
                    _field = true;
                }

                return _field;
            }
            set
            {
                _field = value;
            }
        }

        public bool MultiLineGetterMultiLineSetter
        {
            get
            {
                if (_field == false)
                {
                    _field = true;
                }

                return _field;
            }
            set
            {
                if (_field != value)
                {
                    _field = value;
                }
            }
        }
    }
}