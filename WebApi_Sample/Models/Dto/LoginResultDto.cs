﻿namespace WebApi_Sample.Models.Dto;

public class LoginResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public LoginDataDto Data { get; set; }
}

public class LoginDataDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RequestLoginDto
{
    public string PhoneNumber { get; set; }
    public string SmsCode { get; set; }
}