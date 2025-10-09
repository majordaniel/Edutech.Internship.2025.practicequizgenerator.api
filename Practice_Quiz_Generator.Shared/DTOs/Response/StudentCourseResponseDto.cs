namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class StudentCourseResponseDto
    {
        public string StudentId { get; set; }
        //public string StudentName { get; set; }

        public List<CourseDto> Courses { get; set; } = new();
    }
}
