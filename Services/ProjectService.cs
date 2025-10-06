using Microsoft.EntityFrameworkCore;
using zenBeat.Data;
using zenBeat.DTOs;

namespace zenBeat.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ZenBeatDbContext _context;

        public ProjectService(ZenBeatDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync(string language)
        {
            var projects = await _context.Projects
                .Include(p => p.Language)
                .Include(p => p.ProjectStepsMappings)
                    .ThenInclude(psm => psm.Step)
                    .ThenInclude(s => s.Language)
                .ToListAsync();

            var result = new List<ProjectDto>();

            foreach (var project in projects)
            {
                var projectDto = new ProjectDto
                {
                    Id = project.Id,
                    Name = GetLocalizedText(project.Language, language),
                    Created = project.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = project.CreatedBy,
                    Updated = project.Updated.ToString("yyyy-MM-dd HH:mm:ss"),
                    UpdatedBy = project.UpdatedBy,
                    Steps = new List<StepDto>()
                };

                foreach (var mapping in project.ProjectStepsMappings)
                {
                    var stepDto = new StepDto
                    {
                        Id = mapping.Step.Id,
                        Name = GetLocalizedText(mapping.Step.Language, language),
                        Created = mapping.Step.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                        CreatedBy = mapping.Step.CreatedBy,
                        Updated = mapping.Step.Updated.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedBy = mapping.Step.UpdatedBy
                    };
                    projectDto.Steps.Add(stepDto);
                }

                result.Add(projectDto);
            }

            return result;
        }

        private string GetLocalizedText(Models.Language languageEntity, string language)
        {
            return language.ToLower() switch
            {
                "en-us" => languageEntity.EnUS,
                "zh-cn" => languageEntity.ZhCN,
                "zh-tw" => languageEntity.ZhTW,
                _ => languageEntity.EnUS // Default to English
            };
        }
    }
}