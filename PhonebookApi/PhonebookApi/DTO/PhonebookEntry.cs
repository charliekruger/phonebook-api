using System;
using System.Collections.Generic;

namespace PhonebookApi.DTO
{
    public class PhonebookEntry
    {
        public int PhonebookEntryId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<ContactDetail> ContactDetails { get; set; } = new List<ContactDetail>();
    }
}