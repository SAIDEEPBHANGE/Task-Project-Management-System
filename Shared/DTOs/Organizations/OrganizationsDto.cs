using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Organizations
{
    public class OrganizationsDto
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string OwnerName { get; set; } = null!;
    }
}
