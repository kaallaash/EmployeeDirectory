using EmployeeDirectory.App.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace EmployeeDirectory.App.ViewModels
{
    public class IndexViewModel<T>
    {
        public List<T> Items { get; set; }
        public PageModel PageModel { get; set; }
        public SelectList Filters { get; set; }
        public string FilterName { get; set; }
        public string SearchPhrase { get; set; }
    }
}
