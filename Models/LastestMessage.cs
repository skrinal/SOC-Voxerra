using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Models
{
    public class LastestMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
       
        [JsonProperty("userFriendInfo")]
        public User UserFiendInfo { get; set; }


        public string Content { get; set; }
        public DateTime SendDateTime { get; set; }
        public bool IsRead { get; set; }
    }
}
