﻿namespace Voxerra.Services.Authenticate
{
    public class AuthenticateResponse:BaseResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
