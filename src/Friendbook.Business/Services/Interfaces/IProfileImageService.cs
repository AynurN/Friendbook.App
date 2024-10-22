using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Interfaces
{
    public interface IProfileImageService
    {
        Task<string> UploadProfileImageAsync(IFormFile file, string appUserId);
        Task<string> UpdateProfileImageAsync(IFormFile file, string appUserId);
        Task DeleteProfileImageAsync( string appUserId);

    }
}
