using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.ViewModels
{
    public class FeeCheckReportViewModel
    {
        public virtual FeeCheckGenerateViewModel FeeCheckInput { get; set; }
        public virtual ICollection<FeeCheckListViewModel> FeeCheck { get; set; }
    }
}