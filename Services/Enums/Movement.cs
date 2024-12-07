using System.Runtime.Serialization;

namespace Services.Enums
{
    [DataContract]
    public enum Movement
    {
        [EnumMember]
        None,

        [EnumMember]
        Up,

        [EnumMember]
        Down,

        [EnumMember]
        Left,

        [EnumMember]
        Right,

        [EnumMember]
        RotationCW,

        [EnumMember]
        RotationCCW
    }
}
