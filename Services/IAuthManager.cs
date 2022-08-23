using HotelListing.Dto;

namespace HotelListing.Services;

public interface IAuthManager
{
    Task<bool> ValidateUser(LoginUserDTO userDTO);
    Task<string> CreateToken();
}
