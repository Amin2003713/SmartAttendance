using SmartAttendance.Application.Features.Users.Requests.Commands.Login;

namespace SmartAttendance.Application.Features.Users.Commands.Login;

public class LoginCommand : LoginRequest,
                            IRequest<LoginResponse>;