using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public class CategoryService : ICategoryService
    {

        private IRepositoryWrapper _triviaRepo;

        public CategoryService (IRepositoryWrapper _repo)
        {
            _triviaRepo = _repo;
        }

        public Question GetRandomQuestionById (Guid categoryId)
        {
            var category = _triviaRepo.Category.FindByCondition(x => x.Id == categoryId).First();

            if (category == null) return null;

            var randomQuestion = GetRandomQuestionFromCategory(category);

            return randomQuestion;
        }

        public async Task<Question> GetRandomQuestionByIdAsync (Guid categoryId)
        {
            return await Task.Run(
                () => GetRandomQuestionById(categoryId)
                );
        }

        public Question GetRandomQuestionFromCategory (Category category)
        {
            Random rnd = new Random();

            int length = category.Questions.Count;
            int randomIndex = rnd.Next(0, length - 1);
            var randomQuestion = category.Questions.ElementAt(randomIndex);

            return randomQuestion;
        }

        public async Task<Question> GetRandomQuestionFromCategoryAsync (Category category)
        {
            return await Task.Run(
                () => GetRandomQuestionFromCategory(category)
                );
        }
    }
}
