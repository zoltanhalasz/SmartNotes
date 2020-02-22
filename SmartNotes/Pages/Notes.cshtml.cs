using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartNotes.Helpers;
using SmartNotes.Models;

namespace SmartNotes
{
    public class NotesModel : PageModel
    {

        public int LoginUserID { get; set; }

        public string LoginUserEmail { get; set; }


        public async Task<IActionResult> OnGet()
        {
            //check if the user arrived here using the login page, then having the loginuser properly setup
            var loginuser = SessionHelper.GetObjectFromJson<Users>(HttpContext.Session, "loginuser");
            // if no user logged in using login page, redirect to login
            if (loginuser == null)
            {
                return RedirectToPage("./Login");
            }

            //just pickup the user id and email to show it on the page (and use them in the js code), see html code
            LoginUserID = loginuser.Id;
            LoginUserEmail = loginuser.Email;
            return Page();
        }
    }
}