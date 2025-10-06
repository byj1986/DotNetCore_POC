namespace zenBeat.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StepDto> Steps { get; set; } = new List<StepDto>();
        public string Created { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string Updated { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}