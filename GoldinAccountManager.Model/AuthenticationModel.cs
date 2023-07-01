using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Model
{
    public class AuthenticationModel
    {
        public bool Valid { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

}
