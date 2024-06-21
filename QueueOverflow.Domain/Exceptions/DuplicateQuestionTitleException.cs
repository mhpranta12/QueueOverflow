using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Exceptions
{
	public class DuplicateQuestionTitleException : Exception
	{
		public DuplicateQuestionTitleException() : base("Question Title is duplicate") { }
	}
}
