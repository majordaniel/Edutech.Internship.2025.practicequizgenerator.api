namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class DepartmentResponseDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string HOD { get; set; }
        public string FacultyId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
