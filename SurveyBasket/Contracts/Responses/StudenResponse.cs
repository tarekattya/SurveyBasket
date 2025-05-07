namespace SurveyBasket.Contracts.Responses
{
    public class StudenResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }

        public int Age { get; set; }

        //public string FirstName { get; set; } = string.Empty;
        //public string MiddleName { get; set; } = string.Empty;
        //public string LastName { get; set; } = string.Empty;
    }
}
