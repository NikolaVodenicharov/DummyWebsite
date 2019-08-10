using System.Collections.Generic;
using Tyres.Service.Constants;
using Tyres.Shared.DataTransferObjects.Users;

namespace Tyres.Service.Interfaces
{
    public interface IUserService
    {
        List<UserSummaryDTO> All(int page = PageConstants.DefaultPage);
    }
}
