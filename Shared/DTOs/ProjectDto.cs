using System;
namespace Shared.DTOs;
public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int OrganizationId { get; set; }
    public int ActiveTaskCount { get; set; } // Derived data for the dashboard
}
