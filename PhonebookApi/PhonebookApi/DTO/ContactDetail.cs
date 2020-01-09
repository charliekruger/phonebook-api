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
        public ContactType Type { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// Contact detail type
    /// </summary>
    public enum ContactType
    {
        [Description("Number")]
        Number, 
        [Description("Email Address")]
        EmailAddress
    }
}