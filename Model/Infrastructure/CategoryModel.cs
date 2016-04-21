using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infrastructure
{
    public class CategoryModel
    {
        public IEnumerable<ccategory> Ccategories { get; set; }
        public IEnumerable<rstock> Rstocks { get; set; }
    }
}
