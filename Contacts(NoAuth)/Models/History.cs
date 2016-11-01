using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contacts_NoAuth_.Models
{
    public class History
    {
        public int ID { get; set; }

        public DateTime Time { get; set; }

        public string Action { get; set; }

        public int UserId { get; set; }
    }
}