﻿namespace SelfPay.Mvc.Services.Interface
{
    public interface IUploadService
    {
        Task<string> UploadPhoto(IFormFile file);
    }
}
