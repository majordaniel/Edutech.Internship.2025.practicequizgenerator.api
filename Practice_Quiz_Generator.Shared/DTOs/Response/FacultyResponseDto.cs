namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class FacultyResponseDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Dean { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
