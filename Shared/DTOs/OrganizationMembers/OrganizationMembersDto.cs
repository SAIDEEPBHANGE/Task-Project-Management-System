using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.OrganizationMembers
{
    public class OrganizationMembersDto
    {
        public int Id { get; set; }
        public string OrgName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime? JoinedAt { get; set; }
    }
}
