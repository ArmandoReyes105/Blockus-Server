using System.Runtime.Serialization;

namespace Services.Dtos
{
    [DataContract]
    public class AccountDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int ProfileImage { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
