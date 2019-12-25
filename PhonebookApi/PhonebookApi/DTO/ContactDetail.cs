using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.DTO
{
    public class ContactDetail
    {
        public int ContactDetailId { get; set; }
        public int PhonebookEntryId { get; set; }

        public PhonebookEntry PhonebookEntry { get; set; }
        
        public ContactType Type { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    public enum ContactType
    {
        [Description(nameof(Number))]
        Number, 
        [Description(nameof(EmailAddress))]
        EmailAddress
    }
}