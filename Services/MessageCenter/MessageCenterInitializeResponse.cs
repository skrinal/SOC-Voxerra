using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxerra.Models;

namespace Voxerra.Services.MessageCenter
{
    public class MessageCenterInitializeResponse:BaseResponse
    {
        public User User { get; set; }
        public IEnumerable<User> UserFriends { get; set; }
        public IEnumerable<LastestMessage> LastestMessages { get; set; }
    }
}

