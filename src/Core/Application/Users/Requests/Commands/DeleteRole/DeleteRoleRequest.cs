namespace Shifty.Application.Users.Requests.Commands.DeleteRole;

public class DeleteRoleRequest
{
    public string Role { get; set; }
    public Guid UserId { get; set; }
}