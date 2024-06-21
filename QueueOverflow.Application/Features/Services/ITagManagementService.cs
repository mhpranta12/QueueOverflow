using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
	public interface ITagManagementService
	{
		Task CreateTagAsync(string name);
	}
}
