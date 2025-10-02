namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class CreateUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
        public string CurrentLevelCode { get; set; }
    }
}
