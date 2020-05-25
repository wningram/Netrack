using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Models {
    public class PlayerModel {
        public int Id { get; set; }
        public int SessionId { get; set; }

        public string Location { get; set; }
    }
}
