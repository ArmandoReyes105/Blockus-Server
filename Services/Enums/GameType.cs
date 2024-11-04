using System.Runtime.Serialization;


namespace Services.Enums
{
    [DataContract]
    public enum GameType
    {
        [EnumMember]
        Private,

        [EnumMember]
        Public
    }
}
