using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRIVIA_GAME_API.Services;

namespace TRIVIA_GAME_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IRepositoryWrapper _triviaRepo;
        private IModelCreatorService _model;
        private IPlayerService _playerService;

        private ILoggerManager _logger;


        public PlayersController(
            IRepositoryWrapper repositoryWrapper,
            IModelCreatorService modelCreatorService,
            ILoggerManager logger,
            IPlayerService playerService
            )
        {
            _model = modelCreatorService;
            _triviaRepo = repositoryWrapper;
            _logger = logger;
            _playerService = playerService;
        }

        [HttpGet]
        [Route("leaderboard/{daysPeriod}")]
        public async Task<IActionResult> GetPlyaersByDaysPeriod(int daysPeriod)
        {
            _logger.LogInfo(HttpContext.Request.Query.ToString());

            var result = await _playerService.GetPlayersByDaysPeriodAsync(daysPeriod);
            return Ok(result);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GET (Guid id)
        {
            var player = _triviaRepo.Player.FindByCondition(x => x.Id == id).FirstOrDefault();

            if (player == null) return NotFound();
            
            return Ok(player);
        }

        [HttpGet]
        public IActionResult GET()
        {
            var players = _triviaRepo.Player.FindAll().ToList();
            return Ok(players);
        }

        [HttpPost]
        public async Task<IActionResult> POST([FromBody] Player player)
        {
            Player _player = await _model.CreateOrUpdatePlayerAsync(player);

            if (_player != null)
            {
                return Ok(_player);
            }
            else
            { 
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DELETE(Guid id)
        {
            var player = _triviaRepo.Player.FindByCondition(x => x.Id == id).FirstOrDefault();

            if (player != null)
            {
                _triviaRepo.Player.Delete(player);
                _triviaRepo.Save();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PUT(Guid id, [FromBody]Player player)
        {
            Player oldPlayer = _triviaRepo.Player.FindByCondition(x => x.Id == id).First();
            if (oldPlayer == null)
            {
                return NotFound();
            }
            else 
            {
                var _player = await _model.CreateOrUpdatePlayerAsync(player);
                if (_player != null)
                     return Ok(_player);
                else return BadRequest();
            }
        }

    }
}
