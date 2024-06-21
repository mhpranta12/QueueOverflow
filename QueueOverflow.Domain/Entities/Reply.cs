using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Entities
{
    public class Reply : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
		public DateTime CreationTime { get; set; }
		public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
    }
}
