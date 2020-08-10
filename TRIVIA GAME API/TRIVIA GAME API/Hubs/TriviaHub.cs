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
        private readonly ITriviaHubService _hubService;

        public TriviaHub (ITriviaHubService hubService)
        {
            _hubService = hubService;
        }
        public async Task Send(string message)
        {
            var opponentConnectionId = await _hubService.GetOpponentConnectionIdAsync(Context.ConnectionId);
            await Clients.Client(opponentConnectionId).SendAsync("Send", message);
        }

        public async Task Join (string characterColor)
        {

            var room = await _hubService.TryFindRoomWithOpponentForPlayerAsync(Context.ConnectionId, characterColor);

            var opponentPlayer = room.Players.FirstOrDefault(x => x.ConnectionId != Context.ConnectionId);
            var currentPlayer =  room.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            await Clients.Client(opponentPlayer.ConnectionId).SendAsync("OpponentJoined",
                                                                    currentPlayer.Name,
                                                                    currentPlayer.CharacterColor,
                                                                    currentPlayer.IsGameOrganizer);

            foreach (var player in room.Players)
            { 
                await Clients.Client(player.ConnectionId).SendAsync("CanPlay");
            }
            
        }

        public async Task Leave ()
        {

            var opponentId = await _hubService.GetOpponentConnectionIdAsync(Context.ConnectionId);

            _hubService.DeleteRoomAsync(Context.ConnectionId);
            
            await Clients.Client(opponentId).SendAsync("OpponentLeave");

            
            
        }
    }
}
