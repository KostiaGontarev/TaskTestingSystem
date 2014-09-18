using System;


namespace TTS.Core.Abstract.Model
{
    public interface ITestInfo
    {
        Guid ID { get; }
        string Input { get; set; }
        string Output { get; set; }
    }
}
