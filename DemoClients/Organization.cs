using System.ComponentModel.DataAnnotations;

namespace DemoClients
{
    public class Organization
    {
        public int Id { get; set; } //128bit
        [Display(Name ="Імя")]

        public string Name { get; set; } = default!; // user frendly

        [Display(Name ="FullName")]
        public string FullName { get; set; }= default!;

        //[Range(typeof(DateTime),"01.01.2023","30.01.2023")]
        [Display(Name = "Created")]
        public  DateTime Created { get; set; }
        public IEnumerable<OrganizationUser>? OrganizationUsers { get; set; }

    }
}