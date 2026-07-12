using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{
    public class PremiumMember : Member
    {
        public int MaxBooksAllowed { get; set; } = 10;

        public PremiumMember(int id, string name, string email) : base(id, name, email) { }

        public override string GetMemberType() => "Premium";

        public override string ToString()
        {
            return base.ToString() + $" | Max Books: {MaxBooksAllowed}";
        }
    }

    public class RegularMember : Member
    {
        public RegularMember(int id, string name, string email) : base(id, name, email) { }

        public override string GetMemberType() => "Regular";
    }
}
