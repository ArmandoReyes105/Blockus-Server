using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
