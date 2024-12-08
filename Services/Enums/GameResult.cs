using System.Runtime.Serialization;

namespace Services.Enums
{
    [DataContract]
    public enum GameResult
    {
        [EnumMember]
        None,

        [EnumMember]
        Winner,

        [EnumMember]
        Losser
    }
}
