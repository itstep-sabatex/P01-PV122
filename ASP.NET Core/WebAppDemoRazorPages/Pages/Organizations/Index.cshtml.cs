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
        [BindProperty]
        public string Filter { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Organizations != null)
            {
                Organization = await _context.Organizations.ToListAsync();
            }
        }
 
        public async Task OnPostFilterAsync()
        {
            var containsMethod =typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            if (containsMethod == null) return;
            if (_context.Organizations != null)
            {
                var query = _context.Organizations.AsQueryable(); 
                // get string type property names
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
                            predicate = Expression.Call(Expression.Property(parameter, field),containsMethod, Expression.Constant(Filter));
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
                Organization = await query.ToListAsync();
            }
        }
    }
}
