using KlazzRelationShipFinder.KRSFinder.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Module.Smali
{
    class TempRegister
    {
        public const int TYPE_CONST_STR = 0;

        public const int TYPE_VAR = 1;

        public const int TYPE_METHOD = 2;

        public const int TYPE_OTHER = 999;

        public int TYPE = -1;

        private object value { set; get; }

        public TempRegister(object value) {
            setValue(value);
        }

        private void setType(int TYPE) {
            this.TYPE = TYPE;
        }

        public void setValue(object value) {
            if (value is string)
            {
                setType(TYPE_CONST_STR);
            }
            else if (value is Var)
            {
                setType(TYPE_VAR);
            }
            else if (value is Method)
            {
                setType(TYPE_METHOD);
            }
            this.value = value;
        
        }

        public object getValue() {
            return value;
        }

        public override bool Equals(object t) {
            try {
                TempRegister temp = (TempRegister)t;
                if (temp.TYPE == this.TYPE)
                {
                    if (TYPE == TYPE_CONST_STR)
                    {
                        return temp.value.Equals(this.value);
                    }
                    else if (TYPE == TYPE_VAR)
                    {
                        return ((Var)value).Equals(temp.value);
                    }
                    else if (TYPE == TYPE_METHOD) 
                    {
                        return ((Method)value).Equals(temp.value);
                    }
                }
            }
            catch (Exception) {
                return false;
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
