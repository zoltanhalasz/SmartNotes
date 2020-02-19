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

        private readonly SmartNotes.Models.SmartNotesDBContext _context;

        public NotesModel(SmartNotes.Models.SmartNotesDBContext context)
        {
            _context = context;
        }

        public List<Notes> userNotes { get; set; }

        public int LoginUserID { get; set; }

        public string LoginUserEmail { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var loginuser = SessionHelper.GetObjectFromJson<Users>(HttpContext.Session, "loginuser");
            if (loginuser == null)
            {
                return RedirectToPage("./Login");
            }
            userNotes = await _context.Notes
                 .Where(x => x.Userid == loginuser.Id).OrderByDescending(x=>x.Createdat).ToListAsync();
            LoginUserID = loginuser.Id;
            return Page();
        }
        public async Task<IActionResult> OnGet()
        {
            var loginuser = SessionHelper.GetObjectFromJson<Users>(HttpContext.Session, "loginuser");
            if (loginuser==null)
            {
                return RedirectToPage("./Login");
            }
            userNotes = await  _context.Notes
                  .Where(x => x.Userid == loginuser.Id).OrderByDescending(x => x.Createdat).ToListAsync();
            LoginUserID = loginuser.Id;
            LoginUserEmail = loginuser.Email;
            return Page();
        }
    }
}