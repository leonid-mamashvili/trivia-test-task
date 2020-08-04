using System;
using System.Collections.Generic;
using System.Text;

namespace TRIVIA_GAME_API.Services
{
    public interface IDataCreatorService
    {
        public void CreateTestData(int PlayersCount = 100, int CategorysCount = 10, int QuestionsCount = 10);
    }
}
