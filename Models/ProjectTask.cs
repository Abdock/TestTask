using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models;

#nullable disable

[Table("task")]
public class ProjectTask
{
    [Key, Column("task_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required, Column("task_name"), MaxLength(150)]
    public string Name { get; set; }
    
    [Required, Column("task_description")]
    public string Description { get; set; }
    
    [Required, Column("task_status")]
    public TaskStatus Status { get; set; }
    
    [Required, Column("priority")]
    public int Priority { get; set; }
    
    [ForeignKey(nameof(Project.Id)), Column("project_id")]
    public int ProjectId { get; set; }
}