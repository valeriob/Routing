using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Command
{
    public class Register_New_User
    {
        public string DisplayName { get; set; }
        public string Provider { get; set; }
        public string Identity { get; set; }
    }
}
