using Services.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Services.Dtos
{
    [DataContract]
    public class MatchDTO
    {
        [DataMember]
        public string Host { get; set; }

        [DataMember]
        public string MatchCode { get; set; }

        [DataMember]
        public GameType MatchType { get; set; }

        [DataMember]
        public int NumberOfPlayers { get; set; }

        [DataMember]
        public IEnumerable<Color> ColorsOrder { get; set; }

        [DataMember]
        public List<PublicAccountDTO> Players { get; set; }
    }
}
