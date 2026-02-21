using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.OrganizationMembers
{
    public class UpdateOrganizationMembersDto
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int UserId { get; set; }
        public int Role { get; set; }
    }
}
