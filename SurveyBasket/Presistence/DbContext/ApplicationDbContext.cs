
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SurveyBasket.Excetension;
using System.Security.Claims;

namespace SurveyBasket.Presistence.DbContextt
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options , IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Poll> Polls { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Vote> Votes { get; set; } = null!;
        public DbSet<VoteAnswer> VoteAnswers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

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
