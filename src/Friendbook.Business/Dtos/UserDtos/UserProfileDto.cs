﻿using Friendbook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.UserDtos
{
    public record UserProfileDto(string FullName, string? ProfileImageImageUrl, string Email, string Id);
    
}
