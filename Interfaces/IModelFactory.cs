using TestTask.DTO;
using TestTask.Models;

namespace TestTask.Interfaces;

public interface IModelFactory
{
    Project CreateProject(ProjectDto dto);

    ProjectTask CreateTask(ProjectTaskDto dto);
}