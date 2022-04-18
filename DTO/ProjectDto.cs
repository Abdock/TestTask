namespace TestTask.DTO;

#nullable disable

public class ProjectDto
{
    public string Name { get; set; }
    
    public DateOnly StartDate { get; set; } = DateOnly.Parse(DateTime.Today.ToString("yyyy-MM-dd"));
    
    public DateOnly EndDate { get; set; } = DateOnly.Parse(DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd"));
    
    public int Priority { get; set; }
}