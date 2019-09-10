using System;
using System.ComponentModel.DataAnnotations;

namespace PeopleSearch.DataModel
{
    public partial class People
    {
        [Key]
        public long PeopleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Nullable<long> Age { get; set; }
    }
}
