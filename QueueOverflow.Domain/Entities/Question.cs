using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Entities
{
    public class Question :IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
		public string UserName { get; set; }

		public uint Like {  get; set; }
        public uint Dislike { get; set; }
		public DateTime CreationTime { get; set; }
		public List<Tag> Tags { get; set; }
        public Guid UserId { get; set; }
    }
}
