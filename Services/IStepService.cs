using zenBeat.DTOs;

namespace zenBeat.Services
{
    public interface IStepService
    {
        Task<List<StepDto>> GetStepsAsync(string language);
    }
}