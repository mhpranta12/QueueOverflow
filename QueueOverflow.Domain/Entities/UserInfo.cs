using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Entities
{
    public class UserInfo :IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Designation { get; set; }
        public uint Reputations { get; set; }
        public string? About { get; set; }
        public DateTime JoinedDate { get; set; }
        public string? PortfolioLink { get; set; }
        public string? Address { get; set;}
        public string? GithubLink { get; set; }
        public uint AnswersGiven {  get; set; }
        public uint TotalQuestions {  get; set; }
        public string Badge {  get; set; }
    }
}
