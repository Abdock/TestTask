using TestTask.DTO;
using TestTask.Interfaces;
using TestTask.Models;
using TaskStatus = TestTask.Models.TaskStatus;

namespace TestTask.Services;

public class ModelFactory : IModelFactory
{
    public Project CreateProject(ProjectDto dto)
    {
        return new Project
        {
            ProjectName = dto.Name,
            StartDate = dto.StartDate,
            CompletionDate = dto.EndDate,
            Priority = dto.Priority,
            Status = ProjectStatus.NotStarted
        };
    }

    public ProjectTask CreateTask(ProjectTaskDto dto)
    {
        return new ProjectTask
        {
            Name = dto.Name,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = TaskStatus.ToDo
        };
    }
}