using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.OrganizationMembers;
using Shared.Enums;
using Task_Project_Management_System.Data;
using Task_Project_Management_System.Entities;

namespace Task_Project_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationMembersController : ControllerBase
    {
        ApplicationDBContext _context;
        public OrganizationMembersController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Get/All/OrganizationMembers/{OrgId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrganizationMembersDto>>>> GetAll(int OrgId)
        {
            try
            {
                var members = await _context.OrganizationMembers
                                .Include(User => User.User)
                                .Include(Organization => Organization.Organization)
                                .Where(m => m.OrgId == OrgId)
                                .Select(m => new OrganizationMembersDto
                                {
                                    Id = m.Id,
                                    UserName = m.User.FullName,
                                    OrgName = m.Organization.Name,
                                    Role = m.Role.ToString(),
                                    JoinedAt = m.JoinedAt,
                                }).ToListAsync();
                var response = new ApiResponse<IEnumerable<OrganizationMembersDto>>
                {
                    Success = true,
                    Message = "Organization members retrieved successfully",
                    Data = members
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<OrganizationMembersDto>>
                {
                    Success = false,
                    Message = $"Error retrieving organization members: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("Add/OrganizationMember")]
        public async Task<ActionResult<ApiResponse<OrganizationMembersDto>>> Add(CreateOrganizationMembersDto createDto)
        {
            try
            {
                var newMember = new OrganizationMember
                {
                    UserId = createDto.UserId,
                    OrgId = createDto.OrgId,
                    Role = (OrganizationRole)createDto.Role,
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };
                _context.OrganizationMembers.Add(newMember);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<OrganizationMembersDto>
                {
                    Success = true,
                    Message = "Organization member added successfully",
                    Data = new OrganizationMembersDto
                    {
                        Id = newMember.Id,
                        UserName = (await _context.Users.FindAsync(newMember.UserId)).FullName,
                        OrgName = (await _context.Organizations.FindAsync(newMember.OrgId)).Name,
                        Role = newMember.Role.ToString(),
                        JoinedAt = newMember.JoinedAt
                    }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<OrganizationMembersDto>
                {
                    Success = false,
                    Message = $"Error adding organization member: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        [Route("Update/OrganizationMember/{id}")]
        public async Task<ActionResult<ApiResponse<OrganizationMembersDto>>> Update(int id, UpdateOrganizationMembersDto updateDto)
        {
            try
            {
                var member = await _context.OrganizationMembers.FindAsync(id);
                if (member == null)
                {
                    var notFoundResponse = new ApiResponse<OrganizationMembersDto>
                    {
                        Success = false,
                        Message = "Organization member not found",
                        Data = null
                    };
                    return NotFound(notFoundResponse);
                }
                member.Role = (OrganizationRole)updateDto.Role;
                member.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var response = new ApiResponse<OrganizationMembersDto>
                {
                    Success = true,
                    Message = "Organization member updated successfully",
                    Data = new OrganizationMembersDto
                    {
                        Id = member.Id,
                        UserName = (await _context.Users.FindAsync(member.UserId)).FullName,
                        OrgName = (await _context.Organizations.FindAsync(member.OrgId)).Name,
                        Role = member.Role.ToString(),
                        JoinedAt = member.JoinedAt
                    }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<OrganizationMembersDto>
                {
                    Success = false,
                    Message = $"Error updating organization member: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
        }

        [HttpDelete]
        [Route("Delete/OrganizationMember/{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                var member = await _context.OrganizationMembers.FindAsync(id);
                if (member == null)
                {
                    var notFoundResponse = new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Organization member not found",
                        Data = null
                    };
                    return NotFound(notFoundResponse);
                }
                member.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "Organization member deleted successfully",
                    Data = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error deleting organization member: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
        }
    }
}
