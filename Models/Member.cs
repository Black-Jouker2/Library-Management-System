using Library_Management_System.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{


    public abstract class Member : ISearchable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }

        protected Member(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
            JoinDate = DateTime.Now; 
        }

        public abstract string GetMemberType();

        // تنفيذ واجهة البحث
        public bool MatchesQuery(string query)
        {
            string q = query.ToLowerInvariant();
            return Name.ToLowerInvariant().Contains(q) ||
                   Email.ToLowerInvariant().Contains(q);
        }

        public override string ToString()
        {
            return $"{GetMemberType()} Member #{Id}: {Name} | {Email} | Joined: {JoinDate:yyyy-MM-dd HH:mm}";
        }
    }


}
