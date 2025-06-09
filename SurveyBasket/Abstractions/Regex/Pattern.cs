namespace SurveyBasket.Abstractions.Regex
{
    public static class Pattern
    {
        public static string PasswordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\!@#$%^&*()\[\]{}\-_+=~`|:;""'<>,./?]).{8,}$";
    }
}
