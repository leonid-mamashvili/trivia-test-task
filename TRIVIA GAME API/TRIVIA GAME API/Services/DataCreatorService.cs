using Contracts;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace TRIVIA_GAME_API.Services
{
    public class DataCreatorService : IDataCreatorService
    {
        private readonly IModelCreatorService _model;
        private IConfiguration _config;
        private Random rnd;

        int quantityQuestion;
        int maximumAnswers;
        public DataCreatorService (
            IModelCreatorService model,
            IConfiguration config)
        {
            _model = model;
            _config = config;
            rnd = new Random();
        }
        public void CreateTestData (int PlayersCount = 100, int CategorysCount = 10, int QuestionsCount = 10)
        {
            quantityQuestion = QuestionsCount;
            maximumAnswers = _config.GetValue<int>("MaximumQuestions"); ;
            CreateRandomPlayers(PlayersCount);
            CreateCategories(CategorysCount);
        }

        private void CreateRandomPlayers (int quantity)
        {
            

            int maximumRandomScore = 700; // From task
            int DaysRange = 60;

            string[] colors = new[] { "Green", "Yellow", "Red", "Blue", "Black", "White" };

            for (int i = 0; i < quantity; i++)
            {
                int randomScore = rnd.Next(maximumRandomScore);
                string randomColor = colors[rnd.Next(colors.Length - 1)];
                DateTime randomDatePastTwoMonth = DateTime.Today.AddDays(-rnd.Next(DaysRange));
                
                Player randomPlayer = new Player()
                {
                    Name           = $"Player_{i}",
                    Score          = randomScore,
                    LastGameDate   = randomDatePastTwoMonth,
                    CharacterColor = randomColor
                };

                _model.CreateOrUpdatePlayer(randomPlayer);
            }
        } 

        private void CreateCategories (int quantity)
        {
            string[] categories = new string[] { "Music", "Painting", "Arts", "Architecture", "Dancing", "Photographic", "Graffiti", "Literature", "Theater", "Cinematography" };

            for (int i = 0; i < categories.Length; i++)
            {
                string randomName = categories[rnd.Next(categories.Length - 1)];

                ICollection<Question> randomQuestion = CreateQuestionsForCategory(randomName);

                Category randomCategory = new Category()
                {
                    Name = randomName,
                    Questions = randomQuestion
                };

                _model.CreateOrUpdateCategory(randomCategory);
            }
        }

        private ICollection<Question> CreateQuestionsForCategory (string categoryName)
        {
            List<Question> randomQuestions = new List<Question>();
            for (int i = 0; i < quantityQuestion; i++) 
            {
                var answers = CreateAnswersForQuestion();

                Question randomQuestion = new Question()
                {
                    Text = $"This is the {i} question for the category ‘{categoryName}’",
                    Answers = answers
                };

                randomQuestions.Add(randomQuestion);
            }

            return randomQuestions;
        }

        private ICollection<Answer> CreateAnswersForQuestion ()
        {
            int correctAnswerIndex = rnd.Next(maximumAnswers - 1);
            List<Answer> answers = new List<Answer>();

            for (int i = 0; i < maximumAnswers; i++)
            {
                string randomTextAnswer = "InCorrectAnswer";
                bool isCorrect = false;
                if (i == correctAnswerIndex) {
                    randomTextAnswer = "CorrectAnswer";
                    isCorrect = true; 
                }

                Answer answer = new Answer()
                {
                    Text = randomTextAnswer,
                    IsCorrect = isCorrect
                };

                answers.Add(answer);
            }

            return answers;
        }


    }
}
