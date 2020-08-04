using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public interface ICategoryService
    {
        public Question GetRandomQuestionById(Guid categoryId);

        public Task<Question> GetRandomQuestionByIdAsync(Guid categoryId);

        public Question GetRandomQuestionFromCategory(Category category);

        public  Task<Question> GetRandomQuestionFromCategoryAsync(Category category);
    }
}
