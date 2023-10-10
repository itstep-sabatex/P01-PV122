using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoClients;
using WebAppDemoRazorPages.Data;
using System.Linq.Expressions;
using System.Xml;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public int PageSize { get; set; } = 2;
        [BindProperty(SupportsGet =true)]
        public int Skip { get; set; } = 0;

        public int CountItems { get; set; }
 
        async Task GetItems(IQueryable<Organization> query)
        {
            var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            if (containsMethod == null) return;

            var AvaliableFields = typeof(Organization).GetProperties().Where(p => p.PropertyType == typeof(string)).Select(s => s.Name).ToArray();


            // Define the expression parameters
            ParameterExpression parameter = Expression.Parameter(typeof(Organization), "entity");



            if (Filter != null)
            {
                Expression? predicate = null;
                foreach (var field in AvaliableFields)
                {

                    if (predicate == null)
                    {
                        predicate = Expression.Call(Expression.Property(parameter, field), containsMethod, Expression.Constant(Filter));
                    }
                    else
                    {
                        predicate = Expression.OrElse(predicate, Expression.Call(Expression.Property(parameter, field), containsMethod, Expression.Constant(Filter)));
                    }
                }

                // Create the lambda expression
                Expression<Func<Organization, bool>> lambda = Expression.Lambda<Func<Organization, bool>>(predicate, parameter);
                query = query.Where(lambda);
            }

            CountItems = await query.CountAsync();
            Organization = await query.OrderBy(o=>o.Id).Skip(Skip).Take(PageSize).ToListAsync();
        }


        public async Task OnGetAsync()
        {
            if (_context.Organizations != null)
            {
                await GetItems(_context.Organizations.AsQueryable());
            }
        }
 
        public async Task OnPostFilterAsync()
        {
            if (_context.Organizations != null)
            {
                var query = _context.Organizations.AsQueryable(); 
                // get string type property names
                await GetItems(query);

            }
        }

        public async Task OnPostDeleteAsync(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return ;
            }
            var organization = await _context.Organizations.FindAsync(id);

            if (organization != null)
            {
                
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
            await GetItems(_context.Organizations.AsQueryable()); 
        }

    }
}
