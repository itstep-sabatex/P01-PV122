using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoClients;
using WebAppDemoRazorPages.Data;
// route [namespace:Organizations]/[PageModel:Index]
namespace WebAppDemoRazorPages.Pages.Organizations
{
    public class IndexModel : PageModel
    {
        private readonly WebAppDemoRazorPages.Data.ApplicationDbContext _context;

        public IndexModel(WebAppDemoRazorPages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Organization> Organization { get;set; } = default!;
        [BindProperty]
        public string Filter { get; set; }
        [BindProperty]
        public DirectionSearch DirectionSearch { get; set; }

        public string FieldName { get; set; } = "Name";

        public async Task OnGetAsync()
        {
            if (_context.Organizations != null)
            {
                Organization = await _context.Organizations.ToListAsync();
            }
        }

        public async Task OnPostFilterAsync()
        {
            var query = _context.Organizations.AsQueryable();
            if (Filter != null)
            {
                switch (DirectionSearch)
                {
                    case DirectionSearch.StartWith:
                        query = query.Where(s => s.Name.StartsWith(Filter));
                        break;
                    case DirectionSearch.EndWith:
                        query = query.Where(s => s.Name.EndsWith(Filter));
                        break;
                    case DirectionSearch.Contains:
                        query = query.Where(s => s.Name.Contains(Filter));
                        break;

                }
            }
            Organization = await query.ToListAsync();
        }
    }
}
