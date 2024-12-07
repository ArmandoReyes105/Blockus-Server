using Services.Enums;
using System.Runtime.Serialization;

namespace Services.Dtos
{
    [DataContract]
    public class BlockDTO
    {
        [DataMember]
        public Block Block { get; set; }

        [DataMember]
        public Color Color { get; set; }
    }
}
