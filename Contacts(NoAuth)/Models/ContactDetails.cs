using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contacts_NoAuth_.Models
{
    public class ContactDetails
    {
        public Contacts Contact { get; set; }
        public IEnumerable<History> History { get; set; }
    }
}