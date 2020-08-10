using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public interface ITriviaHubService
    {
        public Task<string> GetOpponentConnectionIdAsync(string playerConnectionId);

        public Task DeleteRoomAsync(string playerConnectionId);

        public Task<GameplayRoom> TryFindRoomWithOpponentForPlayerAsync(string connectionId, string characterColor);
    }
}
