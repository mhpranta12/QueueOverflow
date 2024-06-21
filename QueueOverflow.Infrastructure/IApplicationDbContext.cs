using Microsoft.EntityFrameworkCore;
using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<UserInfo> UserInfo { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Reply> Replies { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Tag> Tags { get; set; }
    }
}
