using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ApiBase.Models
{
    public partial class UsersVr
    {
        public UsersVr()
        {
            AnswersVrs = new HashSet<AnswersVr>();
           
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public string Colony { get; set; }
        public int PostalCode { get; set; }      
        public string City { get; set; }      
        public string State { get; set; }  
        public string Province { get; set; }
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
