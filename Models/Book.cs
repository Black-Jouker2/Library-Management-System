using Library_Management_System.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{
    public class Book : LibraryItem, ISearchable
    {
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Book(int id, string title, string author, int year, string genre)
            : base(id, title)
        {
            Author = author;
            Year = year;
            Genre = genre;
        }

        // تنفيذ واجهة البحث
        public bool MatchesQuery(string query)
        {
            string q = query.ToLowerInvariant();
            return Title.ToLowerInvariant().Contains(q) ||
                   Author.ToLowerInvariant().Contains(q) ||
                   Genre.ToLowerInvariant().Contains(q) ||
                   Year.ToString().Contains(q);
        }

        public override string GetInfo()
        {
            return $"ID: {Id} | {Title} by {Author} ({Year}) [{Genre}] – {(IsAvailable ? "Available" : "Borrowed")}";
        }

        public override string ToString() => GetInfo();
    }
}
