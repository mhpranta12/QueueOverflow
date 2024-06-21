using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Exceptions
{
	public class DuplicateUserNameException : Exception
	{
		public DuplicateUserNameException() : base("UserName is duplicate") { }
	}
}
