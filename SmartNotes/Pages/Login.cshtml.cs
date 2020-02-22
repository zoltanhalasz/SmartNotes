using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartNotes.Models;
using Microsoft.AspNetCore.Http;
using SmartNotes.Helpers;
using Microsoft.EntityFrameworkCore;

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
            // try to find the user in the table having email and password provided
            var myUser = new Users();
            try
            {
                 myUser = await _context.Users.FirstAsync(x => x.Email == LoginUser.Email && x.Password == LoginUser.Password);

            }
            catch (Exception ex)
            {
                errorMessage = "Invalid User/Password";
                // if no user found, error message shown on page in the form.
                return RedirectToPage("./Login", new { err = errorMessage });
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "loginuser", myUser);
            // if user found, it's logged in and redirected to notes.
            return RedirectToPage("./Notes");
        }
    }
}