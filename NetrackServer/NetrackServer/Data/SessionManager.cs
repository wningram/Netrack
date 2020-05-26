using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using NetrackServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetrackServer.Data {
    public class SessionManager : ISessionManager {
        private ApplicationContext _dataContext;
        private IHttpContextAccessor _httpContextAccessor;

        public SessionManager(ApplicationContext dataContext, IHttpContextAccessor http) {
            _dataContext = dataContext;
            _httpContextAccessor = http;
        }

        Map ISessionManager.GetMap(int sessionId) {
           MapModel map = _dataContext.Maps.First(map => map.SessionId == sessionId);
            Map result = new Map(map.MapLocation, map.MapName);
            return result;
        }

        List<Player> ISessionManager.GetPlayers(int sessionId) {
            IEnumerable<Player> results = from player in _dataContext.Players.AsEnumerable().Where(p => p.SessionId == sessionId)
                               select new Player(player.Id, Utilities.parseLocation(player.Location));
            return results.ToList();
        }

        void ISessionManager.SetPlayer(Player player, int sessionId) {
            PlayerModel model = new PlayerModel() {
                Id = player.Id,
                Location = $"{player.Location.X}, {player.Location.Y}",
                SessionId = sessionId
            };
            _dataContext.Players.Update(model);
            _dataContext.SaveChanges();
        }
    }
}
