using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TestTask.Models;

[Table("project")]
public class Project
{
    [Key, Column("project_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required, Column("project_name"), MaxLength(150)]
    public string ProjectName { get; set; }
    
    [Required, Column("start_date"), DefaultValue(typeof(DateTime), "now()")]
    public DateOnly StartDate { get; set; }
    
    [Required, Column("completion_date")]
    public DateOnly CompletionDate { get; set; }

    [Required, Column("project_status")]
    public ProjectStatus Status { get; set; }
    
    [Required, Column("priority")]
    public int Priority { get; set; }
    
    public ICollection<ProjectTask> Tasks { get; set; }

    public Project()
    {
        Tasks = new HashSet<ProjectTask>();
    }
}