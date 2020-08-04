using Contracts;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TRIVIA_GAME_API.Services
{
    public class ModelCreatorService : IModelCreatorService
    {
        private readonly IRepositoryWrapper _triviaRepo;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _config;
        public ModelCreatorService(
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IConfiguration config
            )
        {
            _triviaRepo = repositoryWrapper;
            _logger = logger;
            _config = config;
        }
        public Answer CreateOrUpdateAnswer(Answer answer)
        {
            Answer _answer = _triviaRepo.Answer.FindByCondition(x => answer.Id == x.Id).FirstOrDefault();


            if (_answer == null && answer.QuestionId == null)
            {
                return null;
            }
            else
            {
                int maximumQuestions = _config.GetValue<int>("MaximumQuestions");
                Question question = _triviaRepo.Question.FindByCondition(x => x.Id == _answer.QuestionId).FirstOrDefault();

                List<Answer> answers = _triviaRepo.Question.FindByCondition(x => x.Id == _answer.QuestionId).FirstOrDefault().Answers.ToList();

                _answer.Text = answer.Text;

                if (answer.IsCorrect)
                {
                    foreach (var ans in answers)
                    {
                        if (ans.IsCorrect)
                        {
                            ans.IsCorrect = false;
                        }
                    }

                    _triviaRepo.Answer.Update(_answer);
                   
                }
                else if (question.Answers.Count != maximumQuestions)
                {
                    question.Answers.Add(answer);
                }
                else
                {
                    return null;
                }

                _triviaRepo.Save();
                return _answer;
            }
        }
        

        public async Task<Answer> CreateOrUpdateAnswerAsync (Answer answer)
        {
            return await Task.Run(
                () => CreateOrUpdateAnswer(answer)
                );
        }

        public Category CreateOrUpdateCategory (Category category)
        {
            Category _category = _triviaRepo.Category.FindByCondition(x => category.Id == x.Id).FirstOrDefault();
            //
            // TODO: add validation for category, question and answers
            //
            if (_category == null)
            {
                _triviaRepo.Category.Create(category);
            }
            else
            {
                _triviaRepo.Category.Update(category);
            }
            _triviaRepo.Save();

            return category;
        }

        public async Task<Category> CreateOrUpdateCategoryAsync (Category category)
        {
            return await Task.Run(
                () => CreateOrUpdateCategory(category)
                );
        }

        public GameplayRoom CreateOrUpdateGameplayRoom (GameplayRoom gameplayRoom)
        {
            GameplayRoom _room = _triviaRepo.GameplayRoom.FindByCondition(x => gameplayRoom.Id == x.Id).FirstOrDefault();

            if (_room == null)
            {
                _triviaRepo.GameplayRoom.Create(gameplayRoom);
            }
            else
            {
                _triviaRepo.GameplayRoom.Update(gameplayRoom);
            }
            _triviaRepo.Save();
            return gameplayRoom;
        }

        public async Task<GameplayRoom> CreateOrUpdateGameplayRoomAsync(GameplayRoom gameplayRoom)
        {
            return await Task.Run(
                () => CreateOrUpdateGameplayRoom(gameplayRoom)
                );
        }

        public Player CreateOrUpdatePlayer (Player player)
        {
            Player _player = _triviaRepo.Player.FindByCondition(x => player.Id == x.Id).FirstOrDefault();

            if (_player == null)
            {
                _triviaRepo.Player.Create(player);
            }
            else
            {
                _triviaRepo.Player.Update(_player);
            }
            _triviaRepo.Save();
            return player;
        }

        public async Task<Player> CreateOrUpdatePlayerAsync (Player player)
        {
            return await Task.Run(
                () => CreateOrUpdatePlayer(player)
                );
        }

        public Question CreateOrUpdateQuestion (Question question)
        {
            Question _question = _triviaRepo.Question.FindByCondition(x => question.Id == x.Id).FirstOrDefault();

            if (_question == null)
            {
                _triviaRepo.Question.Create(question);
            }
            else
            {
                _triviaRepo.Question.Update(question);
            }

            _triviaRepo.Save();
            return _question;
        }

        public async Task<Question> CreateOrUpdateQuestionAsync (Question question)
        {
            return await Task.Run(
                () => CreateOrUpdateQuestion(question)
                );
        }
    }
}
