namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class ExcelResponseDto
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; } //= Array.Empty<byte>();
    }
}
