using System.Runtime.Serialization;

namespace Services.Dtos
{
    [DataContract]
    public class ProfileConfigurationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int BoardStyle {  get; set; }
        [DataMember]
        public int TilesStyle { get; set; }
        [DataMember]
        public int IdAccount { get; set; }
    }
}
