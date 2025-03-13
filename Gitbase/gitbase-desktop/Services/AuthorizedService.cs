using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gitbase_desktop.Data;

namespace gitbase_desktop.Services {
    public class AuthorizedService : BaseService {
        protected ApplicationContext context;
        public AuthorizedService(ApplicationContext context) {
            this.context = context;
        }
    }
}
