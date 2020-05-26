using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Models {
    public class PlayerObject {
        public int Id { get; set; }
        public string Location { get; set; }
    }

    public class SessionModel {
        public int Id { get; set; }
        public List<PlayerObject> Players { get; set; }

        public DateTime StartedTime { get; set; }
        public DateTime EndedTime { get; set; }
    }
}
