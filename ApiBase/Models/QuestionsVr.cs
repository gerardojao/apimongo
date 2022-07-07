using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ApiBase.Models
{
    public partial class QuestionsVr
    {
        public QuestionsVr()
        {
            AnswersVrs = new HashSet<AnswersVr>();
        }
        [JsonIgnore]
        public int Id { get; set; }
        public string QuestionDescription { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public DateTime? DeletedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<AnswersVr> AnswersVrs { get; set; }
    }
}
