using Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.Entities;
using NLog;
using RepositoryLayer.DTO;

namespace BookStore.Controllers
{
    /// <summary>
    /// Controller for managing address-related operations such as add, delete, update, and retrieve.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBL _addressBL;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressBL addressBL, ILogger<AddressController> logger)
        {
            _addressBL = addressBL;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the UserId from JWT claims.
        /// </summary>
        /// <returns>UserId as integer</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if UserId claim is missing or invalid</exception>
        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out var userId))
                {
                    _logger.LogInformation($"UserId {userId} successfully retrieved from claims.");
                    return userId;
                }
                else
                {
                    _logger.LogError($"Invalid UserId format in claims: {userIdClaim.Value}");
                    throw new Exception("Invalid UserId format.");
                }
            }
            _logger.LogError("User not authorized, UserId claim is missing.");
            throw new UnauthorizedAccessException("User not authorized.");
        }

        /// <summary>
        /// Adds a new address for the authenticated user.
        /// </summary>
        /// <param name="dto">Address details</param>
        /// <returns>Result of the add operation</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddAddress(AddressDto dto)
        {
            try
            {
                _logger.LogInformation("Attempting to add address...");
                int userId = GetUserIdFromClaims();

                AddressEntity address = new AddressEntity
                {
                    AddressLine = dto.AddressLine,
                    City = dto.City,
                    State = dto.State,
                    Type = dto.Type,
                    Name = dto.Name,
                    MobileNumber = dto.MobileNumber,
                    UserId = userId
                };

                var result = await _addressBL.AddAddress(address);
                if (result)
                {
                    _logger.LogInformation($"Address added successfully for UserId {userId}");
                    return Ok("Address added successfully");
                }
                else
                {
                    _logger.LogWarning($"Failed to add address for UserId {userId}");
                    return BadRequest("Failed to add address");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding address.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an address by its ID.
        /// </summary>
        /// <param name="addressId">The ID of the address to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete address with AddressId {addressId}...");
                var result = await _addressBL.DeleteAddress(addressId);
                if (result)
                {
                    _logger.LogInformation($"Address {addressId} deleted successfully.");
                    return Ok("Address deleted successfully");
                }
                else
                {
                    _logger.LogWarning($"Address {addressId} not found.");
                    return NotFound("Address not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting address.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all addresses for the authenticated user.
        /// </summary>
        /// <returns>List of addresses</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAddresses()
        {
            try
            {
                _logger.LogInformation("Retrieving all addresses...");
                int userId = GetUserIdFromClaims();
                var result = await _addressBL.GetAllAddresses(userId);
                _logger.LogInformation($"Successfully retrieved {result.Count} addresses for UserId {userId}.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all addresses.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="addressDto">Updated address details</param>
        /// <returns>Result of the update operation</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAddress(AddressRequestDto addressDto)
        {
            try
            {
                _logger.LogInformation("Attempting to update address...");
                int userId = GetUserIdFromClaims();

                var addressEntity = new AddressEntity
                {
                    AddressId = addressDto.AddressId,
                    AddressLine = addressDto.AddressLine,
                    City = addressDto.City,
                    State = addressDto.State,
                    Type = addressDto.Type,
                    Name = addressDto.Name,
                    MobileNumber = addressDto.MobileNumber,
                    UserId = userId
                };

                var result = await _addressBL.UpdateAddress(addressEntity);
                if (result)
                {
                    _logger.LogInformation($"Address updated successfully for UserId {userId}");
                    return Ok("Address updated successfully");
                }
                else
                {
                    _logger.LogWarning($"Address {addressDto.AddressId} not found for UserId {userId}");
                    return NotFound("Address not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating address.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }
    }
}
