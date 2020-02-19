using System;
using System.Collections.Generic;

namespace SmartNotes.Models
{
    public partial class Notes
    {
        public Notes()
        {
            Images = new HashSet<Images>();
        }

        public int Id { get; set; }
        public int Userid { get; set; }
        public string Title { get; set; }
        public string NoteText { get; set; }
        public DateTime Createdat { get; set; }
        public bool Pinned { get; set; }

        public string Color { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Images> Images { get; set; }
    }
}
