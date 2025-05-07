﻿namespace SurveyBasket.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public Department Department { get; set; }
    }


    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
