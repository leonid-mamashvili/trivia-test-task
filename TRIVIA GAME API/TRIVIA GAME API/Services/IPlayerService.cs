using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public interface IPlayerService
    {
        public ICollection<Player> GetPlayersByDaysPeriod(int daysPeriod);

        public Task<ICollection<Player>> GetPlayersByDaysPeriodAsync(int daysPeriod);

    }
}
