using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Models {
    public class MapModel {
        public int Id { get; set; }
        public string MapName { get; set; }
        public string MapLocation { get; set; }
        public int SessionId { get; set; }
    }
}
