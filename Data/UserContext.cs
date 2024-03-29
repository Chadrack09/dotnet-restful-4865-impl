﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warp_assessment.Models;

namespace warp_assessment.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) 
            : base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
