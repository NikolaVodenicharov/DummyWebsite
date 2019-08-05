using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Tyres.Data;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Service.Models;

namespace Tyres.Service.Implementations
{
    public class UserService : AbstractService, IUserService
    {
        public UserService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public List<UserSummary> All(int page = 1)
        {
            return base.db
                .Users
                .OrderBy(u => u.UserName)
                .Skip(PageConstants.PageSize * (page - 1))
                .Take(PageConstants.PageSize)
                .ProjectTo<UserSummary>(base.mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
