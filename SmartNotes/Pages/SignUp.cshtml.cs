using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartNotes.Models;

namespace SmartNotes
{
    // used for signup new users
    public class SignUpModel : PageModel
    {
        private readonly SmartNotesDBContext _context;
        public SignUpModel(SmartNotesDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public  Users newUser { get; set; }

        [BindProperty]
        public string errorMessage { get; set; }
        public void OnGet(string err)
        {
            errorMessage = err;
        }

        // basic email validation function
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        bool IsExistingEmail(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        // posting the form on the SignUp page.
        public async Task<IActionResult> OnPost()
        {
            newUser.Email = newUser.Email.Trim();

            if (!IsValidEmail(newUser.Email))
            {
                errorMessage = "Use a valid email address!";
                return RedirectToPage("./SignUp", new { err  =  errorMessage});
            }

            if (IsExistingEmail(newUser.Email))
            {
                errorMessage = "This Email Address has already been used!";
                return RedirectToPage("./SignUp", new { err = errorMessage });
            }

            if (newUser.Password!=newUser.Password2)
            {
                errorMessage = "The passwords do not match!";
                return RedirectToPage("./SignUp", new { err = errorMessage });
            }

            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {   
                // error message is generated and page redirected
                errorMessage = "Error with signup. Please try again later.";
                return RedirectToPage("./SignUp", new { err = errorMessage });
            }

            return RedirectToPage("./Login");
        }

    }
}