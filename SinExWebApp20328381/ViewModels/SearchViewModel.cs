using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20328381.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "The waybill Number must consist of 16 digits!")]
        public string WaybillId { get; set; }

        public SearchViewModel()
        {
            WaybillId = null;
        }
    }
}
