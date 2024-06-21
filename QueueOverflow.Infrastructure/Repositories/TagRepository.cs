using Microsoft.EntityFrameworkCore;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure.Repositories
{
	public class TagRepository : Repository<Tag, Guid>, ITagRepository
	{
		public TagRepository(IApplicationDbContext context) : base((DbContext)context)
		{

		}
	}
}
