namespace TazaFood.Core.Models.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public AppUser User { get; set; } // Navigational Prop [One]
        public string AppUserId { get; set; } // Foreign Key

    }
}