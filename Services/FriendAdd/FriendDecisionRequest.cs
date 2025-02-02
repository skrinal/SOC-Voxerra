using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Services.FriendAdd
{
    public class FriendDecisionRequest
    {
        public int UserRequestToId { get; set; }
        public int UserRequestFromId { get; set; }
        public bool Decision { get; set; }
    }
}
