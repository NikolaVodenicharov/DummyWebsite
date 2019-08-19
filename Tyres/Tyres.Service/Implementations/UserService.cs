using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Users;

namespace Tyres.Service.Implementations
{
    public class UserService : AbstractService, IUserService
    {
        public UserService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public async Task<List<UserSummaryDTO>> AllAsync(int page = 1)
        {
            return await base.db
                .Users
                .OrderBy(u => u.UserName)
                .Skip(PageConstants.PageSize * (page - 1))
                .Take(PageConstants.PageSize)
                .ProjectTo<UserSummaryDTO>(base.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
