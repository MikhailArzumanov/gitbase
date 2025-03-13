using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitbase_desktop.Models {
    public class Token {
        public string   Self   { get; set; } = String.Empty;
        public string[] Roles  { get; set; } = new string[0];
        public int      UserId { get; set; }
    }
}
