using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public class TriviaHubService : ITriviaHubService
    {
        private readonly Dictionary<string, GameplayRoom> _gameplayRooms;
        private readonly IRepositoryWrapper _triviaRepo;
        private readonly ILoggerManager _logger;
        private readonly Random rnd;

        public TriviaHubService (
            IRepositoryWrapper triviaRepo,
            ILoggerManager logger
            )
        {
            _logger = logger;
            _triviaRepo = triviaRepo;
            _gameplayRooms = new Dictionary<string, GameplayRoom>();
            rnd = new Random();
        }

        public async Task<string> GetOpponentConnectionIdAsync (string playerConnectionId)
        {

            var room = _gameplayRooms[playerConnectionId];
            var opponent = room.Players.FirstOrDefault(x => x.ConnectionId != playerConnectionId);

            string resultId = String.Empty;
            if (opponent != null)
                resultId = opponent.ConnectionId;
            
            return resultId;

        }

        public async Task DeleteRoomAsync (string playerConnectionId)
        {
            var room = _gameplayRooms[playerConnectionId];
            var players = room.Players;

            _logger.LogWarn($"{playerConnectionId} leaved room");

            foreach (var player in players)
            {

                player.ConnectionId = null;
                player.IsGameOrganizer = false;
                _triviaRepo.Player.Update(player);
            }
            room.Players = null;

            _logger.LogInfo($"roomId {room.Id} was deleted");
            _triviaRepo.GameplayRoom.Delete(room);
            _triviaRepo.Save();
        }

        public async Task<GameplayRoom> TryFindRoomWithOpponentForPlayerAsync (string connectionId, string characterColor)
        {
            var player = _triviaRepo.Player.FindByCondition(x => x.ConnectionId == connectionId).FirstOrDefault();

            if (player == null)
            {
                player = await AssignConnectionIdToRandomPlayer(connectionId);
            }

            player.CharacterColor = characterColor;
            var room = await GetRoomForPlayer();

            if (room.Players.Count == 0)
            {
                player.IsGameOrganizer = true;    
            }
            else
            {
                player.IsGameOrganizer = false;
               

                var waitingPlayer = room.Players.FirstOrDefault();
                room.Players.Add(player);

                _logger.LogInfo($"PlayerId {player.Id} joined to roomId {room.Id} created by {waitingPlayer.Id}");

            }
            player.LastGameDate = DateTime.Now;


            _logger.LogInfo($"Player with id {player.Id} created room with id {room.Id}");


            _triviaRepo.Player.Update(player);
            _triviaRepo.GameplayRoom.Update(room);
            _gameplayRooms.TryAdd(connectionId, room);
            _triviaRepo.Save();

            return room;
        }

        private async Task<GameplayRoom> GetRoomForPlayer ()
        {
            var room = _triviaRepo.GameplayRoom.FindByCondition(x => x.Players.Count < x.MaxPlayers).FirstOrDefault();

            if (room == null)
            {
                GameplayRoom newRoom = new GameplayRoom()
                {
                    Id = new Guid(),
                    MaxPlayers = 2,
                    Players = new List<Player>()
                };
                room = newRoom;
                _triviaRepo.GameplayRoom.Create(newRoom);
            }

            return room;
        }

        private async Task<Player> AssignConnectionIdToRandomPlayer (string connectionId)
        {
            var playersWithoutConnectionId = _triviaRepo.Player.FindAll().Where(x => x.ConnectionId != null && x.ConnectionId != "").ToList();
            var randomPlayer = playersWithoutConnectionId.ElementAt(rnd.Next(playersWithoutConnectionId.Count - 1));
            randomPlayer.ConnectionId = connectionId;

            return randomPlayer;
        }
    }
}
