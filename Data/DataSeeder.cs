using Microsoft.EntityFrameworkCore;
using zenBeat.Models;

namespace zenBeat.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(ZenBeatDbContext context)
        {
            // Check if data already exists
            if (await context.Languages.AnyAsync())
            {
                return; // Data already seeded
            }

            var currentTime = DateTime.UtcNow;

            // Seed Language data
            var languages = new List<Language>
            {
                new Language
                {
                    Id = 1,
                    Name = "standard_project_name",
                    EnUS = "Standard Project",
                    ZhCN = "标准测试",
                    ZhTW = "繁体标准测试"
                },
                new Language
                {
                    Id = 2,
                    Name = "draw_circles_with_both_hands",
                    EnUS = "Draw circles with both hands",
                    ZhCN = "双手画圈",
                    ZhTW = "繁体双手画圈"
                },
                new Language
                {
                    Id = 3,
                    Name = "draw_circle_with_right_hand",
                    EnUS = "Draw circle with right hand",
                    ZhCN = "右手画圈",
                    ZhTW = "繁体右手画圈"
                },
                new Language
                {
                    Id = 4,
                    Name = "draw_circle_with_left_hand",
                    EnUS = "Draw circle with left hand",
                    ZhCN = "左手画圈",
                    ZhTW = "繁体左手画圈"
                },
                new Language
                {
                    Id = 5,
                    Name = "pat_leg_with_right_hand",
                    EnUS = "pat leg with right hand",
                    ZhCN = "右手拍腿",
                    ZhTW = "繁体右手拍腿"
                },
                new Language
                {
                    Id = 6,
                    Name = "pat_leg_with_left_hand",
                    EnUS = "pat leg with left hand",
                    ZhCN = "左手拍腿",
                    ZhTW = "繁体左手拍腿"
                }
            };

            await context.Languages.AddRangeAsync(languages);
            await context.SaveChangesAsync();

            // Seed Project data
            var project = new Project
            {
                Id = 1,
                Name = "standard_project_name",
                Created = currentTime,
                CreatedBy = "System",
                Updated = currentTime,
                UpdatedBy = "System"
            };

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            // Seed Step data
            var steps = new List<Step>
            {
                new Step
                {
                    Id = 1,
                    Name = "draw_circles_with_both_hands",
                    Created = currentTime,
                    CreatedBy = "System",
                    Updated = currentTime,
                    UpdatedBy = "System"
                },
                new Step
                {
                    Id = 2,
                    Name = "draw_circle_with_right_hand",
                    Created = currentTime,
                    CreatedBy = "System",
                    Updated = currentTime,
                    UpdatedBy = "System"
                },
                new Step
                {
                    Id = 3,
                    Name = "draw_circle_with_left_hand",
                    Created = currentTime,
                    CreatedBy = "System",
                    Updated = currentTime,
                    UpdatedBy = "System"
                },
                new Step
                {
                    Id = 4,
                    Name = "pat_leg_with_right_hand",
                    Created = currentTime,
                    CreatedBy = "System",
                    Updated = currentTime,
                    UpdatedBy = "System"
                },
                new Step
                {
                    Id = 5,
                    Name = "pat_leg_with_left_hand",
                    Created = currentTime,
                    CreatedBy = "System",
                    Updated = currentTime,
                    UpdatedBy = "System"
                }
            };

            await context.Steps.AddRangeAsync(steps);
            await context.SaveChangesAsync();

            // Seed ProjectStepsMapping data
            var projectStepsMappings = new List<ProjectStepsMapping>
            {
                new ProjectStepsMapping { ProjectId = 1, StepId = 1 },
                new ProjectStepsMapping { ProjectId = 1, StepId = 2 },
                new ProjectStepsMapping { ProjectId = 1, StepId = 3 },
                new ProjectStepsMapping { ProjectId = 1, StepId = 4 },
                new ProjectStepsMapping { ProjectId = 1, StepId = 5 }
            };

            await context.ProjectStepsMappings.AddRangeAsync(projectStepsMappings);
            await context.SaveChangesAsync();
        }
    }
}