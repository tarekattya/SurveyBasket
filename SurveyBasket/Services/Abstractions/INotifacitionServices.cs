namespace SurveyBasket.Services.Abstractions
{
    public interface INotifacitionServices
    {
        Task SendNotificationAsync(int? pollid = null);
    }
}
