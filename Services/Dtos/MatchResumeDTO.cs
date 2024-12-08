using Services.MatchState;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Services.Dtos
{
    [DataContract]
    public class MatchResumeDTO
    {
        [DataMember]
        public string MatchCode { get; set; }

        [DataMember]
        public PlayerState Winner { get; set; }

        [DataMember]
        public List<PlayerState> PlayerList { get; set; }

        public int Count { get; set; } = 0; 
    }
}
