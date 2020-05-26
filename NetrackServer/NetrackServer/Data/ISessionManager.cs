using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Data {
    public interface ISessionManager {
        public List<Player> GetPlayers(int sessionId);
        public Map GetMap(int sessionId);
        public void SetPlayer(Player player, int sessionId);
    }
}
