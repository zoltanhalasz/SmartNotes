using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartNotes.Helpers;

namespace SmartNotes
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "loginuser", null);
            return RedirectToPage("./Index");
        }
    }
}