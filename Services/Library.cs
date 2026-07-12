using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Services
{
    public class Library
    {
       
        private int _nextBookId = 1;
        private int _nextMemberId = 1;
        private int _nextRecordId = 1;

        private List<Book> _books = new List<Book>();
        private List<Member> _members = new List<Member>();
        private List<BorrowRecord> _borrowRecords = new List<BorrowRecord>();

        // ---------- إدارة الكتب ----------//
        public void AddBook(string title, string author, int year, string genre)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) ||
                string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Title, author, and genre cannot be empty.");

            var book = new Book(_nextBookId++, title, author, year, genre);
            _books.Add(book);
            Console.WriteLine($"✅ Book '{title}' added with ID {book.Id}.");
        }

        public Book FindBook(int id) => _books.FirstOrDefault(b => b.Id == id);
        public IEnumerable<Book> GetAllBooks() => _books;
        public IEnumerable<Book> GetAvailableBooks() => _books.Where(b => b.IsAvailable);

        // ---------- إدارة الأعضاء ----------//
        public void RegisterMember(string name, string email, string type)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Name and email cannot be empty.");

            Member member;
            if (type.Equals("Premium", StringComparison.OrdinalIgnoreCase))
                member = new PremiumMember(_nextMemberId++, name, email);
            else if (type.Equals("Regular", StringComparison.OrdinalIgnoreCase))
                member = new RegularMember(_nextMemberId++, name, email);
            else
                throw new ArgumentException("Member type must be 'Regular' or 'Premium'.");

            _members.Add(member);
            Console.WriteLine($"✅ {member.GetMemberType()} member '{name}' registered with ID {member.Id} on {member.JoinDate}.");
        }

        public Member FindMember(int id) => _members.FirstOrDefault(m => m.Id == id);
        public IEnumerable<Member> GetAllMembers() => _members;

        // ---------- استعارة كتاب ----------//
        public void BorrowBook(int bookId, int memberId)
        {
            var book = FindBook(bookId);
            if (book == null)
                throw new ArgumentException($"Book with ID {bookId} does not exist.");

            var member = FindMember(memberId);
            if (member == null)
                throw new ArgumentException($"Member with ID {memberId} does not exist.");

            if (!book.IsAvailable)
                throw new InvalidOperationException($"Book '{book.Title}' (ID {bookId}) is currently unavailable.");

            var record = new BorrowRecord(_nextRecordId++, bookId, memberId);
            _borrowRecords.Add(record);
            book.IsAvailable = false;

            Console.WriteLine($"📖 Book '{book.Title}' (ID {bookId}) borrowed by {member.Name} (ID {memberId}) on {record.BorrowDate}.");
        }

        // ---------- إرجاع كتاب ----------//
        public void ReturnBook(int bookId)
        {
            var book = FindBook(bookId);
            if (book == null)
                throw new ArgumentException($"Book with ID {bookId} does not exist.");

            if (book.IsAvailable)
                throw new InvalidOperationException($"Book '{book.Title}' (ID {bookId}) is not currently borrowed.");

            // البحث عن سجل الإعارة المفتوح (الأحدث)
            var record = _borrowRecords
                .Where(r => r.BookId == bookId && !r.ReturnDate.HasValue)
                .OrderByDescending(r => r.BorrowDate)
                .FirstOrDefault();

            if (record == null)
                throw new InvalidOperationException($"No open borrow record found for book ID {bookId}.");

            record.ReturnDate = DateTime.Now;
            book.IsAvailable = true;

            Console.WriteLine($"↩️ Book '{book.Title}' (ID {bookId}) returned on {record.ReturnDate.Value}.");
        }

        // ---------- البحث في الكتالوج ----------//
        public IEnumerable<(object Item, string Type)> SearchCatalog(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<(object, string)>();

            var results = new List<(object, string)>();

            foreach (var book in _books)
                if (book.MatchesQuery(query))
                    results.Add((book, "Book"));

            foreach (var member in _members)
                if (member.MatchesQuery(query))
                    results.Add((member, "Member"));

            return results;
        }

        // ---------- سجل استعارات العضو ----------//
        public IEnumerable<BorrowRecord> GetMemberHistory(int memberId)
        {
            return _borrowRecords.Where(r => r.MemberId == memberId).OrderByDescending(r => r.BorrowDate);
        }

        // ---------- تقرير الكتب المتأخرة ----------//
        public IEnumerable<(string MemberName, string BookTitle, DateTime BorrowDate, int DaysOverdue)> GetLateReturns()
        {
            var lateRecords = _borrowRecords.Where(r => r.IsLate());
            var result = new List<(string, string, DateTime, int)>();

            foreach (var record in lateRecords)
            {
                var member = FindMember(record.MemberId);
                var book = FindBook(record.BookId);
                if (member != null && book != null)
                {
                    result.Add((member.Name, book.Title, record.BorrowDate, record.DaysOverdue()));
                }
            }
            return result;
        }

        // ---------- دوال عرض مساعدة (تستخدمها Program) ----------//
        public void DisplayAvailableBooks()
        {
            var available = GetAvailableBooks().ToList();
            if (!available.Any())
            {
                Console.WriteLine("No books are currently available.");
                return;
            }
            Console.WriteLine("\n--- Available Books ---");
            foreach (var book in available)
                Console.WriteLine(book.GetInfo());
        }

        public void DisplayMemberHistory(int memberId)
        {
            var history = GetMemberHistory(memberId).ToList();
            if (!history.Any())
            {
                Console.WriteLine($"No borrowing history found for member ID {memberId}.");
                return;
            }
            Console.WriteLine($"\n--- Borrowing History for Member {memberId} ---");
            foreach (var record in history)
            {
                var book = FindBook(record.BookId);
                string bookTitle = book?.Title ?? "[Unknown Book]";
                string status = record.ReturnDate.HasValue
                    ? $"Returned on {record.ReturnDate.Value:yyyy-MM-dd}"
                    : (record.IsLate() ? $"LATE (overdue by {record.DaysOverdue()} days)" : "Currently borrowed (on time)");
                Console.WriteLine($"Book: {bookTitle} | Borrowed: {record.BorrowDate:yyyy-MM-dd} | Status: {status}");
            }
        }

        public void DisplayLateReport()
        {
            var lateItems = GetLateReturns().ToList();
            if (!lateItems.Any())
            {
                Console.WriteLine("No overdue items.");
                return;
            }
            Console.WriteLine("\n--- Late Return Report ---");
            foreach (var item in lateItems)
            {
                Console.WriteLine($"Member: {item.MemberName} | Book: {item.BookTitle} | Borrowed: {item.BorrowDate:yyyy-MM-dd} | Days Overdue: {item.DaysOverdue}");
            }
        }
    }
}