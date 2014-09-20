using System;


namespace TTS.Core.Declarations
{
    [Serializable]
    public class Characteristic
    {
        #region Data Members
        private CharacteristicType type;
        private object value;
        #endregion

        #region Properties
        public CharacteristicType Type
        {
            get { return this.type; }
            set
            {
                if (CharacteristicTypeValue.IsBool(value))
                    this.value = CharacteristicTypeValue.DefaultBool;
                else 
                    throw new ArgumentException("That type can not be applicable to a charachteristic!");

                this.type = value;
            }
        }
        public object Value
        {
            get { return this.value; }
            set
            {
                bool boolCompliant = CharacteristicTypeValue.IsBool(this.type) && value is bool;
                if (boolCompliant)
                    this.value = value;
                else
                    throw new ArgumentException("Value does not compliant to this charachteristic type!");
            }
        }
        #endregion

        #region Constructors
        public Characteristic()
        {
            this.type = CharacteristicType.Uknown;
            this.value = new object();
        }
        #endregion
    }
}
