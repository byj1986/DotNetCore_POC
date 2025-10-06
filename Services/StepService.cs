using Microsoft.EntityFrameworkCore;
using zenBeat.Data;
using zenBeat.DTOs;

namespace zenBeat.Services
{
    public class StepService : IStepService
    {
        private readonly ZenBeatDbContext _context;

        public StepService(ZenBeatDbContext context)
        {
            _context = context;
        }

        public async Task<List<StepDto>> GetStepsAsync(string language)
        {
            var steps = await _context.Steps
                .Include(s => s.Language)
                .ToListAsync();

            var result = new List<StepDto>();

            foreach (var step in steps)
            {
                var stepDto = new StepDto
                {
                    Id = step.Id,
                    Name = GetLocalizedText(step.Language, language),
                    Created = step.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = step.CreatedBy,
                    Updated = step.Updated.ToString("yyyy-MM-dd HH:mm:ss"),
                    UpdatedBy = step.UpdatedBy
                };
                result.Add(stepDto);
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