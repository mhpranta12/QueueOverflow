using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Exceptions
{
	public class TagExceedsException:Exception
	{
		public TagExceedsException() : base("Tags are more than 3") { }
	}
}
