using System;
using System.Collections.Generic;

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
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public virtual ICollection<AnswersVr> AnswersVrs { get; set; }
    }
}
