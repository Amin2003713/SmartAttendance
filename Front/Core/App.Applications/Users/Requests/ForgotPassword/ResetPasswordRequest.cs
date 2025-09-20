using MediatR;

namespace App.Applications.Users.Requests.ForgotPassword;

public class ResetPasswordRequest : IRequest
{
    public string UserName { get; set; }


    public string Password { get; set; }


    public string ConfirmPassword { get; set; }
}