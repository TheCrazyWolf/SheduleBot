using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShedulerBotSgk.CustomConsole;

namespace ShedulerBotSgk.ModelDB
{
    internal class DB : DbContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=props.db");
        }


        public DB()
        {
            Write("[DATABASE] Migrate async");
            Database.MigrateAsync();
        }
    }
}
