using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{
    public abstract class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        protected LibraryItem(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public abstract string GetInfo(); // used for display
    }
}
