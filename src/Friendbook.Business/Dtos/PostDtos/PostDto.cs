﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.PostDtos
{
   public record  PostDto(string Content, List<string> PostImageUrls );
    
}
