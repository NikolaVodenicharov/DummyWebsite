using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
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

        public List<UserSummaryDTO> All(int page = 1)
        {
            return base.db
                .Users
                .OrderBy(u => u.UserName)
                .Skip(PageConstants.PageSize * (page - 1))
                .Take(PageConstants.PageSize)
                .ProjectTo<UserSummaryDTO>(base.mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
