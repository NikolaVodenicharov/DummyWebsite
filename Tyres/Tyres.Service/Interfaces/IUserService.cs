using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Service.Constants;
using Tyres.Service.Models;

namespace Tyres.Service.Interfaces
{
    public interface IUserService
    {
        List<UserSummary> All(int page = PageConstants.DefaultPage);
    }
}
