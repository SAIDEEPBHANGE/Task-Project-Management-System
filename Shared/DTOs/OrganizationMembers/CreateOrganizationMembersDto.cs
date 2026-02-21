using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.OrganizationMembers
{
    public class CreateOrganizationMembersDto
    {
        public int OrgId { get; set; }
        public int UserId { get; set; }
        public int Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
