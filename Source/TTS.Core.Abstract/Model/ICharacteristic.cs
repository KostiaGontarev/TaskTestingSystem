using TTS.Core.Abstract.Declarations;


namespace TTS.Core.Abstract.Model
{
    public interface ICharacteristic
    {
        CharacteristicType Type { get; set; }
        object Value { get; set; }
    }
}
