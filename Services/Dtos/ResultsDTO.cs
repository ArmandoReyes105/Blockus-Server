using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    [DataContract]
    public class ResultsDTO
    {

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Victories { get; set; }
        [DataMember]
        public int Losses { get; set; }
        [DataMember]
        public int IdAccount { get; set; }
    }
}
