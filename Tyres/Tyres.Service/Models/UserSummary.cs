using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Models;
using Tyres.Shared.Mappings;

namespace Tyres.Service.Models
{
    public class UserSummary : IMapFrom<User>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
