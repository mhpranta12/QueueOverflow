using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Infrastructure.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid
        , ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    m => m.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //define relations
            builder.Entity<Reply>().ToTable("Replies");
            builder.Entity<UserInfo>().HasKey(x => x.Id);
            builder.Entity<Question>().HasKey(x => x.Id);
            builder.Entity<Reply>().HasKey(x => x.Id);
            builder.Entity<Tag>().HasKey(x => x.Id);
            builder.Entity<Comment>().HasKey(x => x.Id);

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser[]
                {
                    new ApplicationUser { Id = new Guid("54BBFA6A-C47A-4EB5-B244-DCD9EBF9062F"), UserName= "admin@gmail.com", Email = "admin@gmail.com", PasswordHash ="AQAAAAIAAYagAAAAEMZuowgbN90FAp0dQ/Ntby55gPvWCaeKlqfuMgQhyut2hxWl+kbNX3G/Gwa6ayumvA==",SecurityStamp="URNPMVYKJBV6NYGP7M7LOWANMHZCMUUK", EmailConfirmed = true },
                }
                );
            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole[]
                {
                    new ApplicationUserRole{UserId = new Guid("54BBFA6A-C47A-4EB5-B244-DCD9EBF9062F")
                    ,RoleId = new Guid("3A6CD56B-BB0D-4ED5-AFF6-183E54EFBAB8")}
                }
                );
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole[]
                {
                    new ApplicationRole { Id =  new Guid("3A6CD56B-BB0D-4ED5-AFF6-183E54EFBAB8"),Name="Admin",NormalizedName="ADMIN"}
                }
                );
            base.OnModelCreating(builder);
        }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
