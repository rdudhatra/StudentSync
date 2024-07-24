using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace StudentSync.Data.ViewModels
{
    public class ProfileViewModel
    {
        public string? Email { get; set; }
        public string? Username { get; set; }

        public string? Password { get; set; }


    }
}
