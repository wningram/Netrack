using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Models {
    public class ApplicationContext : DbContext {
        public ApplicationContext() : base() { }
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<PlayerModel> Players { get; set; }
        public DbSet<SessionModel> Sessions { get; set; }
        public DbSet<MapModel> Maps { get; set; }
    }
}
