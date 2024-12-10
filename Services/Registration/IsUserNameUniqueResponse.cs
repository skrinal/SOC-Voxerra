using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Services.Registration
{
    public class IsUserNameUniqueResponse : BaseResponse
    {
        public bool IsUserNameUnique { get; set; }
    }
}
