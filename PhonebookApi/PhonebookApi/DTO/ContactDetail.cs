using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.DTO
{
    /// <summary>
    /// Contact detail entry
    /// </summary>
    public class ContactDetail
    {
        public int ContactDetailId { get; set; }
        public int PhonebookEntryId { get; set; }

        public PhonebookEntry PhonebookEntry { get; set; }

        public string Description { get; set; }
        public string Number { get; set; }
    }
}