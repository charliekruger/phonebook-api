using System.Collections.Generic;
using PhonebookApi.DTO;

namespace PhonebookApiTests
{
    public static class TestHelper
    {
        public static List<PhonebookEntry> GetMockEntries()
        {
            return new List<PhonebookEntry>
            {
                new PhonebookEntry
                {
                    Name = "John",
                    Surname = "Smith",
                    ContactDetails = new List<ContactDetail>
                    {
                        new ContactDetail
                        {
                            Type = ContactType.Number,
                            Content = "0821231234",
                            Description = "Mobile"
                        },
                        new ContactDetail
                        {
                            Type = ContactType.EmailAddress,
                            Content = "johnsmith@mail.com",
                            Description = "Personal"
                        }
                    }
                },
                new PhonebookEntry
                {
                    Name = "Jane",
                    Surname = "Johnson",
                    ContactDetails = new List<ContactDetail>
                    {
                        new ContactDetail
                        {
                            Type = ContactType.Number,
                            Content = "0121231234",
                            Description = "Home"
                        },
                        new ContactDetail
                        {
                            Type = ContactType.EmailAddress,
                            Content = "janej@companymail.com",
                            Description = "Work"
                        }
                    }
                },
                new PhonebookEntry
                {
                    Name = "J Jonah",
                    Surname = "Jameson",
                    ContactDetails = new List<ContactDetail>
                    {
                        new ContactDetail
                        {
                            Type = ContactType.Number,
                            Content = "+27721231234",
                            Description = "Work Mobile"
                        },
                        new ContactDetail
                        {
                            Type = ContactType.EmailAddress,
                            Content = "jjj@dailybugle.com",
                            Description = "Work"
                        }
                    }
                },
            };
        }
    }
}