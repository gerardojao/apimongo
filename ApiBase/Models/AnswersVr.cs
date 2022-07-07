using System;
using System.Collections.Generic;

#nullable disable

namespace ApiBase.Models
{
    public partial class AnswersVr
    {
        public int Id { get; set; }
        public string AnswerDescription { get; set; }
        public int? QuestionId { get; set; }
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }

        public virtual QuestionsVr Question { get; set; }
        public virtual UsersVr User { get; set; }
    }
}
