namespace SurveyBasket.Helpers
{
    public static class EmailBodyCreatero
    {
        public static string GenerateBodyEmail(string tempName, Dictionary<string, string> TempModel)
        {
            var TemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{tempName}.html");
            var streamreader = new StreamReader(TemplatePath);
            var body = streamreader.ReadToEnd();
            streamreader.Close();
            foreach (var item in TempModel)
                body = body.Replace(item.Key, item.Value);

            return body;
        }
    }
}

