using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IAnswerRepository Answer { get; }

        ICategoryRepository Category { get; }

        IGameplayRoomRepository GameplayRoom { get; }

        IPlayerRepository Player { get; }

        IQuestionRepository Question { get; }
        void Save();
    }
}
