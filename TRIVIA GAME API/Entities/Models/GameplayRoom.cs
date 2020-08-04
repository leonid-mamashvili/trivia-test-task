using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class GameplayRoom
    {

        public GameplayRoom ()
        {
            Players = new List<Player>();
        }

        public Guid Id { get; set; }

        public int MaxPlayers { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
