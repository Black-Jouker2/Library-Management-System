using Library_Management_System.Models;
using LibraryManagementSystem.Services;
using System;
using System.Reflection;
namespace LibraryManagementSystem
{
    class Program
    {
                                  
        private static Library library = new Library();

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Library Management System ===");
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Register a Member");
                Console.WriteLine("3. Borrow a Book");
                Console.WriteLine("4. Return a Book");
                Console.WriteLine("5. Search the Catalog");
                Console.WriteLine("6. View Available Books");
                Console.WriteLine("7. Member Borrowing History");
                Console.WriteLine("8. Late Return Report");
                Console.WriteLine("9. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddBook(); break;
                        case "2": RegisterMember(); break;
                        case "3": BorrowBook(); break;
                        case "4": ReturnBook(); break;
                        case "5": SearchCatalog(); break;
                        case "6": library.DisplayAvailableBooks(); break;
                        case "7": MemberHistory(); break;
                        case "8": library.DisplayLateReport(); break;
                        case "9": exit = true; Console.WriteLine("Goodbye!"); break;
                        default: Console.WriteLine("Invalid option. Please try again."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
        }

        // ---------- دوال الإدخال ----------
        static void AddBook()
        {
            Console.Write("Enter title: ");
            string title = Console.ReadLine()?.Trim();
            Console.Write("Enter author: ");
            string author = Console.ReadLine()?.Trim();
            Console.Write("Enter year: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
                throw new ArgumentException("Year must be a valid number.");
            Console.Write("Enter genre: ");
            string genre = Console.ReadLine()?.Trim();

            library.AddBook(title, author, year, genre);
        }

        static void RegisterMember()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine()?.Trim();
            Console.Write("Enter email: ");
            string email = Console.ReadLine()?.Trim();
            Console.Write("Enter type (Regular/Premium): ");
            string type = Console.ReadLine()?.Trim();

            library.RegisterMember(name, email, type);
        }

        static void BorrowBook()
        {
            Console.Write("Enter book ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Invalid book ID.");
            Console.Write("Enter member ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
                throw new ArgumentException("Invalid member ID.");

            library.BorrowBook(bookId, memberId);
        }

        static void ReturnBook()
        {
            Console.Write("Enter book ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Invalid book ID.");

            library.ReturnBook(bookId);
        }

        static void SearchCatalog()
        {
            Console.Write("Enter search query: ");
            string query = Console.ReadLine()?.Trim();
            var results = library.SearchCatalog(query);

            if (!results.Any())
            {
                Console.WriteLine("No results found.");
                return;
            }

            Console.WriteLine($"\n--- Search Results for '{query}' ---");
            foreach (var item in results)
            {
                if (item.Type == "Book")
                    Console.WriteLine($"[Book] {((Book)item.Item).GetInfo()}");
                else if (item.Type == "Member")
                    Console.WriteLine($"[Member] {item.Item}");
            }
        }

        static void MemberHistory()
        {
            Console.Write("Enter member ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
                throw new ArgumentException("Invalid member ID.");

            library.DisplayMemberHistory(memberId);
        }
    }
}
