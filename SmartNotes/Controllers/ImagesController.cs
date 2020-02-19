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
    public class ImagesController : ControllerBase
    {
        private readonly SmartNotesDBContext _context;

        public ImagesController(SmartNotesDBContext context)
        {
            _context = context;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Images>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Images>> GetImages(int id)
        {
            var images = await _context.Images.FindAsync(id);

            if (images == null)
            {
                return NotFound();
            }

            return images;
        }

        [HttpGet("imagesbynote/{noteid}")]

        public async Task<ActionResult<List<Images>>> GetImagesByNote(int noteid)
        {
            var images = await _context.Images.Where(x=> x.Noteid ==noteid).ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return images;
        }



        [HttpGet("imagesbyuser/{userid}")]

        public async Task<ActionResult<List<Images>>> GetImagesByUser(int userid)
        {
            var images =  _context.Images.ToList();

                if (images == null)
                {
                    return NotFound();
                }

                return images;
        }


        // PUT: api/Images/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImages(int id, Images images)
        {
            if (id != images.Id)
            {
                return BadRequest();
            }

            _context.Entry(images).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagesExists(id))
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

        // POST: api/Images
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<Images>> PostImages(Images images)
        //{
        //    _context.Images.Add(images);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetImages", new { id = images.Id }, images);
        //}

     

        [HttpPost("uploadimage/{noteid}")]
        public async Task<ActionResult<Images>> PostUpload( int noteid, IFormFile image)
        {

            if (image != null && noteid!=0 && image.Length > 0)
            {                 
                        //Getting FileName
                        var fileName = Path.GetFileName(image.FileName);

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        // Combines two strings into a path.
                        var filepath =
                        new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            image.CopyTo(fs);
                            fs.Flush();
                        }

                        var newImage = new Images();
                        newImage.Image = newFileName;
                        newImage.Noteid = noteid;
                        _context.Images.Add(newImage);
                        await _context.SaveChangesAsync();                 
        

                var myImageList = await _context.Images.Where(x => x.Noteid == noteid).ToListAsync();

                return Ok(myImageList);
            }

            return NoContent();
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Images>> DeleteImages(int id)
        {
            var images = await _context.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }

            _context.Images.Remove(images);
            await _context.SaveChangesAsync();

            return images;
        }


        [HttpDelete("deleteimagesbynote/{noteid}")]
        public async Task<ActionResult<Images>> DeleteImagesByNote(int noteid)
        {
            var images = await _context.Images.Where(x=> x.Noteid == noteid).ToListAsync();
            if (images == null)
            {
                return NotFound();
            }

            foreach (var img in images)
            {
                deleteImage(img.Image);
                _context.Images.Remove(img);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private void deleteImage(string imagefile)
        {
            var filepath =             
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")).Root + $@"\{imagefile}";
            
            System.IO.File.Delete(filepath);
       
        }

        private bool ImagesExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
