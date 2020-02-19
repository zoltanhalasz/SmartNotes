using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SmartNotes.Models;

namespace SmartNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly SmartNotesDBContext _context;

        public NotesController(SmartNotesDBContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notes>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
        }

        // GET: api/Notes/5     
        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> GetNotes(int id)
        {
            var notes = await _context.Notes.FindAsync(id);

            if (notes == null)
            {
                return NotFound();
            }

            return notes;
        }

        [HttpGet("notesbyuser/{userid}/{order}/{searchstring}")]
        public async Task<ActionResult<List<Notes>>> GetNotesByUser(int userid, string order="Desc", string searchstring="")
        {
            var notes = new List<Notes>();
            if (searchstring == "(empty)") searchstring = "";
            searchstring = searchstring.ToLower();           
            if (order=="Desc")
            {
                notes = await _context.Notes.Where(x => x.Userid == userid).OrderBy(x => x.Pinned).ThenByDescending(x=>x.Createdat).ToListAsync();
            }
            else
            {
                notes = await _context.Notes.Where(x => x.Userid == userid).OrderBy(x => x.Pinned).ThenBy(x => x.Createdat).ToListAsync();
            }

            if (notes == null)
            {
                return NotFound();
            }

            return  notes.Where(x=> x.Title.ToLower().Contains(searchstring) || x.NoteText.ToLower().Contains(searchstring)).ToList();
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotes(int id, Notes notes)
        {
            if (id != notes.Id)
            {
                return BadRequest();
            }

            _context.Entry(notes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Notes>> PostNotes(Notes notes)
        {         
                _context.Notes.Add(notes);
                await _context.SaveChangesAsync();
            return CreatedAtAction("PostNotes", new { id = notes.Id }, notes);
        }

        [HttpPost("pinnote/{noteid}")]

        public async Task<ActionResult<Notes>> PinNote(int noteid)
        {
            var myNote = await _context.Notes.FindAsync(noteid);
            if (myNote!=null)
            {
                myNote.Pinned = !myNote.Pinned;
                _context.Notes.Update(myNote);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }


        [HttpPut("changecolor/{noteid}")]

        public async Task<ActionResult<Notes>> ChangeColor(int noteid, Notes notes)
        {
            var myNote = await _context.Notes.FindAsync(noteid);
            if (myNote != null)
            {
                myNote.Color = notes.Color;
                _context.Notes.Update(myNote);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }


        [HttpPut("updatenote/{noteid}")]

        public async Task<ActionResult<Notes>> UpdateNote(int noteid, Notes notes)
        {
            var myNote = await _context.Notes.FindAsync(noteid);
            if (myNote != null)
            {
                myNote.Title = notes.Title;
                myNote.NoteText = notes.NoteText;
                _context.Notes.Update(myNote);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }


        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Notes>> DeleteNotes(int id)
        {

            var images = await _context.Images.Where(x => x.Noteid == id).ToListAsync();

            foreach (var img in images)
            {
                var filepath =
                       new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")).Root + $@"\{img.Image}";
                System.IO.File.Delete(filepath);
            }
            if (images!=null) _context.Images.RemoveRange(images);
            var notes = await _context.Notes.FindAsync(id);
            if (notes == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
