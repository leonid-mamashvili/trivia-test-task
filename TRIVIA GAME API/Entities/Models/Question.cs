using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class Question
    {
        public Question ()
        {
            Answers = new List<Answer>();
        }

        public Guid Id { get; set; }

        public string Text { get; set; }

        public ICollection<Answer> Answers { get; set; }

        // [JsonIgnore] can be used
        public Category Category { get; set; }
    }
}
