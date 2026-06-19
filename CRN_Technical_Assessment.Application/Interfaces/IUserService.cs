using CRN_Technical_Assessment.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Application.Interfaces
{
    public interface IUserService
    { 
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id); 
        Task<UserDto?> GetByEmailAsync(string email);
        Task<int> CreateAsync(RegisterUserDto dto);
        Task UpdateProfileAsync(int userId, RegisterUserDto dto);
        Task DeleteAsync(int id); Task RestoreAsync(int id);
    }
}
