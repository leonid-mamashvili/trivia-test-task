using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Category
    {
        public Category ()
        {
            Questions = new List<Question>();
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
