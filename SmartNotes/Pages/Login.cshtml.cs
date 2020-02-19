using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartNotes.Models;
using Microsoft.AspNetCore.Http;
using SmartNotes.Helpers;

namespace SmartNotes
{
    public class LoginModel : PageModel
    {

        private readonly SmartNotesDBContext _context;
        public LoginModel(SmartNotesDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Users LoginUser { get; set; }

        [BindProperty]
        public string errorMessage { get; set; }
 

        public void OnGet(string err)
        {
            errorMessage = err;
        }

        public async Task<IActionResult> OnPost()
        {
            var myUser = new Users();
            try
            {
                 myUser = _context.Users.First(x => x.Email == LoginUser.Email && x.Password == LoginUser.Password);

            }
            catch (Exception ex)
            {
                errorMessage = "Invalid User/Password";
                return RedirectToPage("./Login", new { err = errorMessage });
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "loginuser", myUser);
            return RedirectToPage("./Notes");
        }
    }
}