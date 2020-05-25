using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Data {
    public interface ISessionManager {
        List<Player> GetPlayers(int sessionId);
        Map GetMap(int sessionId);
        void SetPlayer(Player player, int sessionId);
    }
}
