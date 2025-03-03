using Microsoft.AspNetCore.Mvc;

namespace Backend.Constants {
    public class Responses {
        public static readonly IActionResult FORBIDDEN = new StatusCodeResult(403);

        public static readonly IActionResult INTERNAL_ERROR  = new StatusCodeResult(500);
        public static readonly IActionResult NOT_IMPLEMENTED = new StatusCodeResult(501);
    }
}
