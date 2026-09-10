using System.Security.Claims;

namespace exam_system.Features.Shared;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContext(IHttpContextAccessor httpContextAccessor)
        {
        _httpContextAccessor = httpContextAccessor;
        }

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        return parsedUserId;
    }
}