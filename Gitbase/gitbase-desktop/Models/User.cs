using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitbase_desktop.Models {
    public class User {
        public int    Id       { get; set; }
        public string Authname { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;
        public string Email    { get; set; } = String.Empty;
        public string About    { get; set; } = String.Empty;
        public string Company  { get; set; } = String.Empty;
        public string Links    { get; set; } = String.Empty;

    }
}
