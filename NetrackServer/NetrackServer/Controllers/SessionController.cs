using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using NetrackServer.Models;

namespace NetrackServer.Controllers {
    public class SessionController : Controller {
        private ApplicationContext _dbContext;
        private int _sessionId;

        public SessionController(ApplicationContext context) {
            _dbContext = context;
        }
        [HttpPost]
        public int StartSession() {
            SessionModel session = new SessionModel() {
                StartedTime = DateTime.Now
            };
            EntityEntry entry = _dbContext.Sessions.Add(session);
            _sessionId = entry.CurrentValues.GetValue<int>("Id");
            _dbContext.SaveChanges();
            return _sessionId;
        }

        public IActionResult EndSession(int sessionId) {
            SessionModel session = _dbContext.Sessions.Find(sessionId);
            session.EndedTime = DateTime.Now;
            _dbContext.Sessions.Update(session);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}