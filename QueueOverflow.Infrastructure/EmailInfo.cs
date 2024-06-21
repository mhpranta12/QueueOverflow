using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure
{
	public class EmailInfo
	{
		public Guid Id { get; set; }
		public string EmailOfUser { get; set; }
		public EmailInfo() { }
	}
}
