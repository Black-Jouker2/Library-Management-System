using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{

    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } // null = لم يُرجع بعد

        private const int LoanPeriodDays = 14; // مدة الإعارة 14 يوماً

        public BorrowRecord(int id, int bookId, int memberId)
        {
            Id = id;
            BookId = bookId;
            MemberId = memberId;
            BorrowDate = DateTime.Now;
            ReturnDate = null;
        }

        // هل الكتاب متأخر؟
        public bool IsLate()
        {
            if (ReturnDate.HasValue)
                return false; // تم إرجاعه
            return BorrowDate.AddDays(LoanPeriodDays) < DateTime.Now;
        }

        // عدد الأيام المتأخرة (في حال التأخير)
        public int DaysOverdue()
        {
            if (!IsLate()) return 0;
            return (int)(DateTime.Now - BorrowDate.AddDays(LoanPeriodDays)).TotalDays;
        }

        public override string ToString()
        {
            string status = ReturnDate.HasValue
                ? $"Returned on {ReturnDate.Value:yyyy-MM-dd}"
                : (IsLate() ? $"LATE (overdue by {DaysOverdue()} days)" : "Borrowed (on time)");
            return $"Record #{Id}: Book {BookId} borrowed by Member {MemberId} on {BorrowDate:yyyy-MM-dd} – {status}";
        }
    }
}
