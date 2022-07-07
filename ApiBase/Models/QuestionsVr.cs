using System;
using System.Collections.Generic;

#nullable disable

namespace ApiBase.Models
{
    public partial class QuestionsVr
    {
        public QuestionsVr()
        {
            AnswersVrs = new HashSet<AnswersVr>();
        }

        public int Id { get; set; }
        public string QuestionDescription { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<AnswersVr> AnswersVrs { get; set; }
    }
}
