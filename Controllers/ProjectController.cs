using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask.DTO;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : Controller
{
    private readonly ILogger<ProjectController> _logger;
    private readonly DatabaseContext _database;
    private readonly IModelFactory _modelFactory;

    public ProjectController(ILogger<ProjectController> logger, DatabaseContext context, IModelFactory factory)
    {
        _logger = logger;
        _database = context;
        _modelFactory = factory;
    }

    [HttpGet]
    [Route("get/{id:int}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        // Get project and load the related tasks
        return Json(await _database.Projects.Where(project => project.Id == id)
            .Include(project => project.Tasks)
            .FirstOrDefaultAsync());
    }

    [HttpPost, Route("create")]
    public async Task<ActionResult<Project>> CreateProject([FromBody] ProjectDto dto)
    {
        // Check project to correct
        if (dto.EndDate < dto.StartDate)
        {
            return BadRequest(new {errorText = "Project completion date less than start date"});
        }

        if (dto.Priority <= 0)
        {
            return BadRequest(new {errorText = "Project priority must be greater 0"});
        }
        
        if (await _database.Projects.AnyAsync(project => project.ProjectName == dto.Name))
        {
            return BadRequest(new {errorText = "Project with given name already exists"});
        }
        
        var project = _modelFactory.CreateProject(dto);
        await _database.Projects.AddAsync(project);
        await _database.SaveChangesAsync();
        return Ok(project);
    }

    [HttpPost, Route("add/task")]
    public async Task<ActionResult<Project>> CreateProjectTask([FromBody] ProjectTaskDto dto)
    {
        // Check task
        if (dto.Priority <= 0)
        {
            return BadRequest(new {errorText = "Task priority must be greater 0"});
        }

        var project = await _database.Projects.Where(project => project.ProjectName == dto.ProjectName)
            .Include(project => project.Tasks).FirstOrDefaultAsync();
        if (project == null)
        {
            return NotFound(new {errorText = "Project by given id not found"});
        }

        // If user add new task to completed project, it's mean project active now
        if (project.Status == ProjectStatus.Completed)
        {
            project.Status = ProjectStatus.Active;
        }

        var task = _modelFactory.CreateTask(dto);
        project.Tasks.Add(task);
        _database.Update(project);
        await _database.SaveChangesAsync();
        return Json(project);
    }

    [HttpPost, Route("modify/{id:int}")]//reveice project id because, name may be changed
    public async Task<ActionResult<Project>> ModifyProject([FromRoute] int id, [FromBody] ProjectDto dto)
    {
        var project = await _database.Projects.Where(product => product.Id == id)
            .Include(product => product.Tasks).FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound(new {errorText = "Project by given id not found"});
        }

        project.ProjectName = dto.Name ?? project.ProjectName; // if name is null assign previous value
        project.Priority = dto.Priority;
        project.StartDate = dto.StartDate;
        project.CompletionDate = dto.EndDate;
        
        _database.Update(project);
        await _database.SaveChangesAsync();
        
        return Json(project);
    }

    [HttpPost, Route("modify/{projectId:int}/task/{taskId:int}")]
    public async Task<ActionResult<Project>> ModifyTask([FromRoute] int projectId, [FromRoute] int taskId,
        [FromBody] ProjectTaskDto dto)
    {
        var project = await _database.Projects.Where(product => product.Id == projectId)
            .Include(project => project.Tasks).FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound(new {errorText = "Product by given id not found"});
        }

        if (project.Tasks.All(task => task.Id != taskId))
        {
            return NotFound(new {errorText = "Task by given id not found"});
        }

        var task = project.Tasks.First(task => task.Id == taskId);

        task.Name = dto.Name ?? task.Name; // if name is null return previous name
        task.Priority = dto.Priority;
        task.Description = dto.Description ?? task.Description; // if description is null return previous name

        _database.Update(project);
        await _database.SaveChangesAsync();

        return Json(project);
    }

    [HttpPost, Route("remove/{id:int}")]
    public async Task<ActionResult> RemoveProject([FromRoute] int id)
    {
        var project = await _database.Projects.Where(project => project.Id == id)
            .Include(project => project.Tasks).FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound(new {errorText = "Project by given id not found"});
        }

        _database.Remove(project);
        await _database.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost, Route("remove/{projectId:int}/task/{taskId:int}")]
    public async Task<ActionResult> RemoveProject([FromRoute] int projectId, [FromRoute] int taskId)
    {
        var task = await _database.Tasks.FirstOrDefaultAsync(task => task.Id == taskId && task.ProjectId == projectId);

        if (task == null)
        {
            return NotFound(new {errorText = "Task by given id not found"});
        }

        _database.Remove(task);
        await _database.SaveChangesAsync();

        return Ok();
    }
}