using Tyres.Data.Models;
using Tyres.Shared.Mappings;

namespace Tyres.Shared.DataTransferObjects.Users
{
    public class UserSummaryDTO : IMapFrom<User>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
