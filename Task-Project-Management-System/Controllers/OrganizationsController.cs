using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Project_Management_System.Data;
using Task_Project_Management_System.Entities;

namespace Task_Project_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public OrganizationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAll/Organizations")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrganizationsDto>>>> GetAll()
        {
            try
            {
                var orgs = await _context.Organizations
                    .Include(o => o.Owner)
                    .Select(o => new OrganizationsDto
                    {
                        Name = o.Name,
                        Slug = o.Slug,
                        OwnerName = o.Owner.FullName
                    })
                    .ToListAsync();
                var response = new ApiResponse<IEnumerable<OrganizationsDto>>
                {
                    Success = true,
                    Message = "Organizations retrieved successfully.",
                    Data = orgs
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving organizations. {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet]
        [Route("Get/Organization/{id}")]
        public async Task<ActionResult<ApiResponse<OrganizationsDto>>> GetById(int id)
        {
            try
            {
                var org = await _context.Organizations
                    .Include(o => o.Owner)
                    .Where(o => o.Id == id)
                    .Select(o => new OrganizationsDto
                    {
                        Name = o.Name,
                        Slug = o.Slug,
                        OwnerName = o.Owner.FullName
                    })
                    .FirstOrDefaultAsync();
                if (org == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Organization not found.",
                        Data = null
                    });
                }
                var response = new ApiResponse<OrganizationsDto>
                {
                    Success = true,
                    Message = "Organization retrieved successfully.",
                    Data = org
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the organization. {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPost]
        [Route("Create/Organization")]
        public async Task<ActionResult<ApiResponse<OrganizationsDto>>> create(CreateOrganizationsDto createDto)
        {
            try
            {
                var owner = await _context.Users.FindAsync(createDto.OwnerId);
                if (owner == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Owner not found.",
                        Data = null
                    });
                }
                var org = new Organization
                {
                    Name = createDto.Name,
                    Slug = createDto.Slug,
                    OwnerId = createDto.OwnerId,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.Organizations.Add(org);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<OrganizationsDto>
                {
                    Success = true,
                    Message = "Organization created successfully.",
                    Data = new OrganizationsDto
                    {
                        Name = org.Name,
                        Slug = org.Slug,
                        OwnerName = owner.FullName
                    }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred while creating the organization. {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPut]
        [Route("Update/Organization")]
        public async Task<ActionResult<ApiResponse<OrganizationsDto>>> update(UpdateOrganizationsDto updateDto)
        {
            try
            {
                var org = await _context.Organizations.FindAsync(updateDto.Id);
                if (org == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Organization not found.",
                        Data = null
                    });
                }
                var owner = await _context.Users.FindAsync(updateDto.OwnerId);
                if (owner == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Owner not found.",
                        Data = null
                    });
                }
                org.Name = updateDto.Name;
                org.Slug = updateDto.Slug;
                org.OwnerId = updateDto.OwnerId;
                org.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var response = new ApiResponse<OrganizationsDto>
                {
                    Success = true,
                    Message = "Organization updated successfully.",
                    Data = new OrganizationsDto
                    {
                        Name = org.Name,
                        Slug = org.Slug,
                        OwnerName = owner.FullName
                    }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred while updating the organization. {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpDelete]
        [Route("Delete/Organization/{id}")]
        public async Task<ActionResult<ApiResponse<string>>> delete(int id)
        {
            try
            {
                var org = await _context.Organizations.FindAsync(id);
                if (org == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Organization not found.",
                        Data = null
                    });
                }
                org.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "Organization deleted successfully.",
                    Data = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred while deleting the organization. {ex.Message}",
                    Data = null
                });
            }
        }
    }
}
