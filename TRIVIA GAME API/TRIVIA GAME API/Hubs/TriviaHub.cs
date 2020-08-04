using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TRIVIA_GAME_API.Controllers;
using TRIVIA_GAME_API.Services;

namespace TRIVIA_GAME_API.Hubs
{
    public class TriviaHub : Hub
    {
        private readonly IRepositoryWrapper _triviaRepo;
        private readonly Random rnd;
        private readonly Dictionary<string, GameplayRoom> _gameplayRooms;
        private readonly ILoggerManager _logger;
        private readonly ITriviaHubService _hubService;

        public TriviaHub (
            IRepositoryWrapper repo,
            ILoggerManager logger,
            ITriviaHubService hubService)
        {
            _logger = logger;
            _triviaRepo = repo;
            _hubService = hubService;

            rnd = new Random();
            _gameplayRooms = new Dictionary<string, GameplayRoom>();
        }
        public async Task Send (string message)
        {
            var room = _gameplayRooms[Context.ConnectionId];
            var anotherPlayers = room.Players.Where(x => x.ConnectionId != Context.ConnectionId);

            foreach (var player in anotherPlayers)
            {
                await Clients.Client(player.ConnectionId).SendAsync("Send", message);
            }
        }

        public async Task Join (string characterColor)
        {
            var player = _triviaRepo.Player.FindByCondition(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();

            if (player == null)
            {
                var randomPlayer = _triviaRepo.Player.FindAll().ElementAt(rnd.Next(_triviaRepo.Player.FindAll().ToList().Count - 1));
                randomPlayer.ConnectionId = Context.ConnectionId;
                
                player = randomPlayer;
             //   _triviaRepo.Save();
            }

            var room = _triviaRepo.GameplayRoom.FindByCondition(x => x.Players.Count < x.MaxPlayers).FirstOrDefault();
            player.CharacterColor = characterColor;

            if (room == null)
            {
                GameplayRoom newRoom = new GameplayRoom()
                {
                    Id = new Guid(),
                    MaxPlayers = 2,
                    Players = new List<Player>() { player }
                };

                _logger.LogInfo($"Player with id {player.Id} created room with id {newRoom.Id}");

                player.IsGameOrganizer = true;
                player.LastGameDate = DateTime.Now;

                _gameplayRooms.TryAdd(Context.ConnectionId, newRoom);

                _triviaRepo.GameplayRoom.Create(newRoom);
                _triviaRepo.Player.Update(player);
            }
            else
            {
                player.IsGameOrganizer = false;
                player.LastGameDate = DateTime.Now;

                var waitingPlayer = room.Players.FirstOrDefault();
                room.Players.Add(player);

                _logger.LogInfo($"PlayerId {player.Id} joined to roomId {room.Id} created by {waitingPlayer.Id}");

                _gameplayRooms.TryAdd(Context.ConnectionId, room);

                await Clients.Client(waitingPlayer.ConnectionId).SendAsync("OpponentJoined",
                                                                                    player.Name,
                                                                                    player.CharacterColor,
                                                                                    player.IsGameOrganizer);

                await Clients.Client(player.ConnectionId).SendAsync("CanPlay");
                await Clients.Client(waitingPlayer.ConnectionId).SendAsync("CanPlay");
            }

            _triviaRepo.Save();
        }

        public async Task Leave ()
        {
            var room = _gameplayRooms[Context.ConnectionId];
            var players = room.Players;

            _logger.LogWarn($"{Context.ConnectionId} leaved room");

            foreach (var player in players)
            {
                await Clients.Client(player.ConnectionId).SendAsync("OpponentLeave");
                player.ConnectionId = null;
                player.IsGameOrganizer = false;
                _triviaRepo.Player.Update(player);
            }
            room.Players = null;

            _logger.LogInfo($"roomId {room.Id} was deleted");
            _triviaRepo.GameplayRoom.Delete(room);
            _triviaRepo.Save();
        }
    }
}
