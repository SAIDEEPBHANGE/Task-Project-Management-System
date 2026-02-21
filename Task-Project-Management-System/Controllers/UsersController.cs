using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Users;
using Task_Project_Management_System.Data;
using Task_Project_Management_System.Entities;

namespace Task_Project_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UsersController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsersDto>>>> GetAll()
        {
            try
            {
                var users = await _context.Users
                            .Select(u => new UsersDto
                            {
                                Id = u.Id,
                                FullName = u.FullName,
                                Username = u.Username,
                                Email = u.Email,
                                AvatarUrl = u.AvatarUrl
                            })
                            .ToListAsync();
                var response = new ApiResponse<IEnumerable<UsersDto>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = users
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<UsersDto>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving users: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult<ApiResponse<UsersDto>>> Create(CreateUsersDto createUserDto)
        {
            try
            {
                var user = new User
                {
                    FullName = createUserDto.FullName,
                    Username = createUserDto.Username,
                    Email = createUserDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                    AvatarUrl = createUserDto.AvatarUrl
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var userDto = new UsersDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    AvatarUrl = user.AvatarUrl
                };
                return Ok(ApiResponse<UsersDto>.SuccessResponse(userDto, "User created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UsersDto>.FailureResponse($"An error occurred while creating the user: {ex.Message}"));
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult<ApiResponse<UsersDto>>> Update(UpdateUsersDto updateUserDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(updateUserDto.Id);
                if (user == null)
                {
                    return NotFound(ApiResponse<UsersDto>.FailureResponse("User not found"));
                }
                user.FullName = updateUserDto.FullName;
                user.Email = updateUserDto.Email;
                user.AvatarUrl = updateUserDto.AvatarUrl;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var userDto = new UsersDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    AvatarUrl = user.AvatarUrl
                };
                return Ok(ApiResponse<UsersDto>.SuccessResponse(userDto, "User updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UsersDto>.FailureResponse($"An error occurred while updating the user: {ex.Message}"));
            }
        }

        [HttpPut]
        [Route("ChangePassword/{id}")]
        public async Task<ActionResult<ApiResponse<UsersDto>>> ChangePassword(int id, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse<UsersDto>.FailureResponse("User not found"));
                }
                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest(ApiResponse<UsersDto>.FailureResponse("Current password is incorrect"));
                }
                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
                {
                    return BadRequest(ApiResponse<UsersDto>.FailureResponse("New password and confirmation do not match"));
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var userDto = new UsersDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    AvatarUrl = user.AvatarUrl
                };
                return Ok(ApiResponse<UsersDto>.SuccessResponse(userDto, "Password changed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UsersDto>.FailureResponse($"An error occurred while changing the password: {ex.Message}"));
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse.FailureResponse("User not found"));
                }
                user.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok(ApiResponse.SuccessResponse(null, "User deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.FailureResponse($"An error occurred while deleting the user: {ex.Message}"));
            }
        }
    }
}