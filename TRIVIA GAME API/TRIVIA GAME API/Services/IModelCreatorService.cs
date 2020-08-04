using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public interface IModelCreatorService
    {
        public Player CreateOrUpdatePlayer (Player player);
        public Task<Player> CreateOrUpdatePlayerAsync (Player player);

        public Answer CreateOrUpdateAnswer (Answer answer);
        public Task<Answer> CreateOrUpdateAnswerAsync (Answer answer);

        public Category CreateOrUpdateCategory (Category category);
        public Task<Category> CreateOrUpdateCategoryAsync (Category category);

        public GameplayRoom CreateOrUpdateGameplayRoom (GameplayRoom gameplayRoom);
        public Task<GameplayRoom> CreateOrUpdateGameplayRoomAsync (GameplayRoom gameplayRoom);

        public Question CreateOrUpdateQuestion (Question question);
        public Task<Question> CreateOrUpdateQuestionAsync(Question question);


    }
}
