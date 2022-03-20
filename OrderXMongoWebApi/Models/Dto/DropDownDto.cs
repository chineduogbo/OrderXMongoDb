using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Models.Dto
{
    public class DropDownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    };
    public class ReturnLoginDto
    {
        public string Username { get; set; }
        public string Authkey { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
    };

}
