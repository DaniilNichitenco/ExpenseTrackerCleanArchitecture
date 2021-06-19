using System;
using ExpenseTracker.Core.Domain.Auth;
using ExpenseTracker.Core.Domain.Schemas;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repository.IdentityServer
{
    public class IDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public IDbContext(DbContextOptions<IDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyIdentityMapConfiguration(builder);
        }

        private void ApplyIdentityMapConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users", IdentitySchemas.Auth);
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", IdentitySchemas.Auth);
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", IdentitySchemas.Auth);
            modelBuilder.Entity<UserToken>().ToTable("UserRoles", IdentitySchemas.Auth);
            modelBuilder.Entity<Role>().ToTable("Roles", IdentitySchemas.Auth);
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", IdentitySchemas.Auth);
            modelBuilder.Entity<UserRole>().ToTable("UserRole", IdentitySchemas.Auth);
        }
    }
}
