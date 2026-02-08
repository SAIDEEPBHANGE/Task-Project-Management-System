using System;
using Shared.Enums;
namespace Shared.DTOs;
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssigneeName { get; set; }
    public int ProjectId { get; set; }
}
