
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace SurveyBasket.Presistence.DbContextt
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options , IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);



        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entityentry in entries)
            {
                if (entityentry.State == EntityState.Added)
                {
                    entityentry.Entity.CreatedById = currentUserId!;
                }
                else if (entityentry.State == EntityState.Modified)
                {
                    entityentry.Entity.UpdatedById = currentUserId;
                    entityentry.Entity.UpdatedOn = DateTime.UtcNow;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
