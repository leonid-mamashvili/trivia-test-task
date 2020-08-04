using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public class QuestionService : IQuestionService
    {
        private ICategoryService _categoryService;
        private IRepositoryWrapper _triviaRepo;

        public QuestionService(
            IRepositoryWrapper repo,
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public Question GetRandomQuestionByCategoryId (Guid categoryId)
        {
            //TODO check is cateogry exist on controller
            var category = _triviaRepo.Category.FindByCondition(x => x.Id == categoryId).First();

            if (category == null)
            {
                return null;
            }

            var randomQuestion = _categoryService.GetRandomQuestionFromCategory(category);

            return randomQuestion;
        }
    }
}
