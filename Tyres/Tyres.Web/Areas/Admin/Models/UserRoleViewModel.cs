using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tyres.Web.Areas.Admin.Models
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Roles")]
        public IList<string> CurrentRoles { get; set; }

        public string ChosedRole { get; set; }

        [Display(Name = "Choose role")]
        public IEnumerable<SelectListItem> ChooseRoles { get; set; }
    }
}
