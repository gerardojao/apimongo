using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace ApiBase.Models
{
    public partial class UsersVr
    {
        public UsersVr()
        {
            QuestionsVrs = new HashSet<QuestionsVr>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }       
        public string Email { get; set; }
        [JsonIgnore]
        public string VerificationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? QuestionId { get; set; }

        public virtual ICollection<QuestionsVr> QuestionsVrs { get; set; }
    }
}
