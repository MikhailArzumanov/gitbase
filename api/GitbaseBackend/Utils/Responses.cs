using Microsoft.AspNetCore.Mvc;

namespace GitbaseBackend.Utils {
    public class Responses {
        public static readonly IActionResult NOT_IMPLEMENTED = new StatusCodeResult(501);
    }
}
