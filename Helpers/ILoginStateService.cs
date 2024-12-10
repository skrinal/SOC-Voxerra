using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Helpers
{
    public interface ILoginStateService
    {
        bool IsLoggedIn { get; set; }
        void Logout();
    }
}
