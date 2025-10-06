using zenBeat.DTOs;

namespace zenBeat.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjectsAsync(string language);
    }
}