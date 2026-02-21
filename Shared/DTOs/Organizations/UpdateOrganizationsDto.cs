using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Organizations
{
    public class UpdateOrganizationsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int OwnerId { get; set; }
    }
}
