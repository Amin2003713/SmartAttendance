﻿namespace SmartAttendance.Application.Features.Users.Requests.Commands.Verify;

public class VerifyPhoneNumberRequest
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}