namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class BulkUserUploadDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string RegistrationNumber { get; set; }
        public string DepartmentId { get; set; }
        public string FacultyId { get; set; }
        public string CurrentLevelId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
