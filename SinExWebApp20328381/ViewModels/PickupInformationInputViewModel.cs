using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.ViewModels
{
    public class PickupInformationInputViewModel
    {
        public virtual DropdownListsViewModel SystemOutputSource { get; set; }
        public virtual string PickupBuildingAddress { get; set; }
        [Required]
        public virtual string PickupStreetAddress { get; set; }
        [Required]
        public virtual string PickupCityAddress { get; set; }
        [Required]
        public virtual string ServiceCity { get; set; }
        [Required]
        public virtual string PickupType { get; set; }
        public virtual DateTime ShippedDate { get; set; }
    }
}