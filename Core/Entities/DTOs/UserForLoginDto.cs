using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.DTOs
{
    public class UserForLoginDto

    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }

    }

}