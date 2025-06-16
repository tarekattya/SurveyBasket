
using Microsoft.AspNetCore.Identity.UI.Services;
using SurveyBasket.Helpers;
using SurveyBasket.Presistence.DbContextt;
using System.Collections;

namespace SurveyBasket.Services
{
    public class NotifacitionServices(ApplicationDbContext context 
        , UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender  ) : INotifacitionServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task SendNotificationAsync(int? pollid = null)
        {
            IEnumerable<Poll> polls = [];

            if (pollid.HasValue)
            {
                var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == pollid.Value && p.IsPublished);

                polls = [poll!];
            }
            else
            {
                polls = await _context.Polls
                    .Where(p => p.IsPublished && p.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                    .AsNoTracking()
                    .ToListAsync();
            }
            var users =await  _userManager.Users.ToListAsync();
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            foreach (var pol in polls) {
                foreach (var user in users)
                {
                    var Placeholder = new Dictionary<string, string>
                    {
                        { "{{name}}", user.FirstName },
                        { "{{pollTill}}", pol.Title },
                        { "{{endDate}}", pol.EndsAt.ToString() },
                        { "{{url}}", $"{origin}/polls/statrs/{pol.Id}" }
                    };

                    var body = EmailBodyCreatero.GenerateBodyEmail("PollNotification",Placeholder);
                    await _emailSender.SendEmailAsync(user.Email!, "New Poll Notification", body);
                }
            
            }

        }
    }
}
