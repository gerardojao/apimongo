using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ApiBase.Models
{
    public partial class AnswersVr
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string AnswerDescription { get; set; }
        public int? QuestionId { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdateAt { get; set; }
        [JsonIgnore]
        public DateTime? DeleteAt { get; set; }
        [JsonIgnore]
        public virtual QuestionsVr Question { get; set; }
        [JsonIgnore]
        public virtual UsersVr User { get; set; }
    }
}
