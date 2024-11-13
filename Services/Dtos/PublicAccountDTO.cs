using System.Runtime.Serialization;

namespace Services.Dtos
{
    [DataContract]
    public class PublicAccountDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public int ProfileImage { get; set; }
    }
}
