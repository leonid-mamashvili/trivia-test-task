using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class GameplayRoomRepository : RepositoryBase<GameplayRoom>, IGameplayRoomRepository
    {
        public GameplayRoomRepository(TriviaContext context) : base(context)
        {

        }
    }
}
