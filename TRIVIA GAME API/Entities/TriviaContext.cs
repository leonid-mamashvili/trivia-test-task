using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class TriviaContext : DbContext
    {
        public TriviaContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<GameplayRoom> GameplayRooms { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Question> Questions { get; set; }
    }
}
