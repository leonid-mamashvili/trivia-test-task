using Contracts;
using Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public class PlayerService : IPlayerService
    {
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;

        public PlayerService (
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger
            )
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }
        public ICollection<Player> GetPlayersByDaysPeriod (int daysPeriod)
        {
            if (daysPeriod < 0)
            {
                throw new Exception("Days Period should be more than 0");
            }

            var LastGameDateTime = DateTime.Now.AddDays(-daysPeriod);
            var players = _repositoryWrapper.Player.FindByCondition(x => x.LastGameDate > LastGameDateTime).ToList();
            
            return players;
        }

        public async Task<ICollection<Player>> GetPlayersByDaysPeriodAsync(int daysPeriod)
        {
            return await Task.Run(
                () => GetPlayersByDaysPeriod(daysPeriod)
                );
        }
    }
}
