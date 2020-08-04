using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Player
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Score { get; set; }

        public DateTime LastGameDate { get; set; }

        public bool IsLastGameDate { get; set; }

        public bool IsGameOrganizer { get; set; }

        public string ConnectionId { get; set; }

        public string CharacterColor { get; set; }

    }
}
