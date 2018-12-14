using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVProof.Models;
using CVProof.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CVProof.Web.Models
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel() { }

        public UserProfileViewModel(UserProfileModel profile)
        {
            Id = profile.Id;
            Roles = profile.Roles.Split(',').ToList();
            Name = profile.Name;
            Image = profile.Image;

            RolesList = new List<SelectListItem>();

            foreach (string role in Enum.GetNames(typeof(CVProof.Models.Role)))
            {
                RolesList.Add(new SelectListItem { Value = role, Text = role });
            }
        }

        public string Id { get; set; }

        public string Name { get; set; }
        public List<string> Roles { get; set; }

        public List<int> Roles2 { get; set; }

        public List<SelectListItem> RolesList { get; set; }

        public string Key { get; set; }

        public byte[] Image { get; set; }

        public IFormFile Files { get; set; }


        public UserProfileModel GetProfileModel()
        {
            UserProfileModel ret = new UserProfileModel();

            ret.Id = this.Id;
            ret.Name = this.Name;
            ret.Roles = String.Join(',', this.Roles);
            ret.Image = this.Image;

            return ret;
        }
       
    }
}
