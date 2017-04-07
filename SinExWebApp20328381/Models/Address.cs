using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    public class Address
    {
        public virtual string NickName { get; set; }
        public virtual int AddressId { get; set; }
        public virtual int ShippingAccountId { get; set; }
        public virtual string Value { get; set; }
    }
}