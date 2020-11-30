using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class MysqlContext : DbContext
    {
        public DbSet<Subjects> Subjects { get; set; }
        public MysqlContext(DbContextOptions<MysqlContext> options) : base(options)
        {

        }

    }
}
