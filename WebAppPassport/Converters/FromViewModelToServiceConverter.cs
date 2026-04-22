using WebAppPassport.Controllers.Models;
using ServiceModels = WebAppPassport.Services.Models;

namespace WebAppPassport.Converters;

/// <summary>
/// Converts presentation-layer view models into service-layer models.
/// Simple scalar parameters (e.g. isoShortCode) are passed directly without conversion.
/// </summary>
public static class FromViewModelToServiceConverter
{
    /// <summary>
    /// Creates a service User from a registration request.
    /// Password hashing is intentionally left to the service layer.
    /// </summary>
    public static ServiceModels.User ToServiceModel(this RegisterRequest request) =>
        new() { Username = request.Username, HashedPassword = request.Password };

    /// <summary>
    /// Extracts credentials tuple from a login request.
    /// </summary>
    public static (string Username, string Password) ToCredentials(this LoginRequest request) =>
        (request.Username, request.Password);

    /// <summary>
    /// Extracts credentials tuple from a register request.
    /// </summary>
    public static (string Username, string Password) ToCredentials(this RegisterRequest request) =>
        (request.Username, request.Password);
}
