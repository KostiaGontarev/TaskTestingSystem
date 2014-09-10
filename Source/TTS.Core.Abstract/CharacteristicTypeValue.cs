using TTS.Core.Abstract.Declarations;

namespace TTS.Core.Abstract
{
    public static class CharacteristicTypeValue
    {
        #region Constants
        public const bool DefaultBool = false;
        public const double DefaultDouble = 0.0;
        #endregion

        #region Members
        public static bool IsBool(CharacteristicType type)
        {
            switch (type)
            {
                case CharacteristicType.InputOutputCompliance:
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsDouble(CharacteristicType type)
        {
            switch (type)
            {
                case CharacteristicType.Uknown:
                case CharacteristicType.InputOutputCompliance:
                    return false;
                default:
                    return false;
            }
        }
        #endregion
    }
}
