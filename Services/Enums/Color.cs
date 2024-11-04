using System.Runtime.Serialization;

namespace Services.Enums
{
    [DataContract]
    public enum Color
    {
        [EnumMember]
        Red,

        [EnumMember]
        Blue,

        [EnumMember]
        Yellow,

        [EnumMember]
        Green
        
    }
}
