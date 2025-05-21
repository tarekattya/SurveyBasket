namespace SurveyBasket.Presistence.Entites
{
    public class AuditableEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedById { get; set; }=string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;


    }
}
