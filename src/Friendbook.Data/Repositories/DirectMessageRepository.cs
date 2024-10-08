using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Data.Repositories
{
    public class DirectMessageRepository : GenericRepository<DirectMessage>, IDirectMessageRepository
    {
        public DirectMessageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
