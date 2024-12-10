using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Services.Registration
{
    public class IsUserNameUniqueRequest
    {
        public string UserName { get; set; } = null!;
    }
}
