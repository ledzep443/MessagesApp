﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateOnly HireDate { get; set; }
    }
}
