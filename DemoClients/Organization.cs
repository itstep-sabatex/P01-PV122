using System.ComponentModel.DataAnnotations;

namespace DemoClients
{
    public class Organization
    {
        public int Id { get; set; } //128bit
        [Display(Name ="Імя")]
        public string Name { get; set; } = default!; // user frendly
        public string FullName { get; set; }=default!;
        public IEnumerable<OrganizationUser>? OrganizationUsers { get; set; }

    }
}