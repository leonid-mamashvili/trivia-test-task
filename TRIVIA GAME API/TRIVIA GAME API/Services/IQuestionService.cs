using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRIVIA_GAME_API.Services
{
    public interface IQuestionService
    {
        public Question GetRandomQuestionByCategoryId(Guid categoryId);
    }
}
