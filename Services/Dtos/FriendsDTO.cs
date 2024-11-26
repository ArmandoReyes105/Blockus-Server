using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    [DataContract]
    public class FriendsDTO
    {
        [DataMember]
        public int IdFriend { get; set; }
        [DataMember]
        public int IdAccount { get; set; }
        [DataMember]
        public int IdAccountFriend {get; set; }
    }
}
