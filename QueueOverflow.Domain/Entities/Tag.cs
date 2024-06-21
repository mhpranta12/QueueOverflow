using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Entities
{
    public class Tag :IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid QuestionId { get; set; }
    }
}
