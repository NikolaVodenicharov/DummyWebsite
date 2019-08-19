using System.Collections.Generic;
using System.Threading.Tasks;
using Tyres.Service.Constants;
using Tyres.Shared.DataTransferObjects.Users;

namespace Tyres.Service.Interfaces
{
    public interface IUserService
    {
        Task<List<UserSummaryDTO>> AllAsync(int page = PageConstants.DefaultPage);
    }
}
