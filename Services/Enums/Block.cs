using System.Runtime.Serialization;

namespace Services.Enums
{
    [DataContract]
    public enum Block
    {
        [EnumMember]
        Block01,

        [EnumMember]
        Block02,

        [EnumMember]
        Block03
    }
}
