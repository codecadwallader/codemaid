namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBetweenMultiLinePropertyAccessors
    {
        private bool _field;
        
        public bool AutoImplementedProperty { get; set; }

        public bool SingleLineProperty
        {
            get { return _field; }
            set { _field = value; }
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

        public bool MultiLineSetterMultiLineGetter
        {
            set
            {
                if (_field != value)
                {
                    _field = value;
                }
            }

            get
            {
                if (_field == false)
                {
                    _field = true;
                }

                return _field;
            }
        }

        public bool MultiLineGetterMultiLineSetter2
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

        public bool MultiLineSetterMultiLineGetter2
        {
            set
            {
                if (_field != value)
                {
                    _field = value;
                }
            }

            get
            {
                if (_field == false)
                {
                    _field = true;
                }

                return _field;
            }
        }
    }
}
