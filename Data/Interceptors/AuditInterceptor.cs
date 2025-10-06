using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using zenBeat.Models;
using zenBeat.Models.History;
using System.Reflection;

namespace zenBeat.Data.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context != null)
            {
                CreateAuditEntries(eventData.Context);
            }
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                CreateAuditEntries(eventData.Context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void CreateAuditEntries(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity is Language language)
                {
                    CreateLanguageAudit(context, entry, language);
                }
                else if (entry.Entity is Project project)
                {
                    CreateProjectAudit(context, entry, project);
                }
                else if (entry.Entity is Step step)
                {
                    CreateStepAudit(context, entry, step);
                }
                else if (entry.Entity is ProjectStepsMapping mapping)
                {
                    CreateProjectStepsMappingAudit(context, entry, mapping);
                }
            }
        }

        private void CreateLanguageAudit(DbContext context, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, Language language)
        {
            if (entry.State == EntityState.Modified)
            {
                var modifiedProperties = entry.Properties.Where(p => p.IsModified && p.Metadata.Name != "Id").ToList();
                
                foreach (var property in modifiedProperties)
                {
                    var audit = new LanguageHistory
                    {
                        LanguageId = language.Id,
                        FieldName = property.Metadata.Name,
                        FromValue = property.OriginalValue?.ToString() ?? "",
                        ToValue = property.CurrentValue?.ToString() ?? "",
                        ChangedAt = DateTime.UtcNow,
                        UpdatedBy = "System" // TODO: Get from current user context
                    };
                    context.Set<LanguageHistory>().Add(audit);
                }
            }
        }

        private void CreateProjectAudit(DbContext context, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, Project project)
        {
            if (entry.State == EntityState.Modified)
            {
                var modifiedProperties = entry.Properties.Where(p => p.IsModified && p.Metadata.Name != "Id").ToList();
                
                foreach (var property in modifiedProperties)
                {
                    var audit = new ProjectHistory
                    {
                        ProjectId = project.Id,
                        FieldName = property.Metadata.Name,
                        FromValue = property.OriginalValue?.ToString() ?? "",
                        ToValue = property.CurrentValue?.ToString() ?? "",
                        ChangedAt = DateTime.UtcNow,
                        UpdatedBy = "System" // TODO: Get from current user context
                    };
                    context.Set<ProjectHistory>().Add(audit);
                }
            }
        }

        private void CreateStepAudit(DbContext context, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, Step step)
        {
            if (entry.State == EntityState.Modified)
            {
                var modifiedProperties = entry.Properties.Where(p => p.IsModified && p.Metadata.Name != "Id").ToList();
                
                foreach (var property in modifiedProperties)
                {
                    var audit = new StepHistory
                    {
                        StepId = step.Id,
                        FieldName = property.Metadata.Name,
                        FromValue = property.OriginalValue?.ToString() ?? "",
                        ToValue = property.CurrentValue?.ToString() ?? "",
                        ChangedAt = DateTime.UtcNow,
                        UpdatedBy = "System" // TODO: Get from current user context
                    };
                    context.Set<StepHistory>().Add(audit);
                }
            }
        }

        private void CreateProjectStepsMappingAudit(DbContext context, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, ProjectStepsMapping mapping)
        {
            var audit = new ProjectStepsMappingHistory
            {
                ProjectId = mapping.ProjectId,
                StepId = mapping.StepId,
                FieldName = entry.State.ToString(),
                FromValue = entry.State == EntityState.Added ? "" : $"ProjectId:{mapping.ProjectId},StepId:{mapping.StepId}",
                ToValue = entry.State == EntityState.Deleted ? "" : $"ProjectId:{mapping.ProjectId},StepId:{mapping.StepId}",
                ChangedAt = DateTime.UtcNow,
                UpdatedBy = "System" // TODO: Get from current user context
            };
            context.Set<ProjectStepsMappingHistory>().Add(audit);
        }
    }
}