using System.Collections.Generic;
using Model;

namespace AgroFirma.Models
{
    public class CategoryModel
    {
        public IEnumerable<ccategory> Ccategories { get; set; }
        public IEnumerable<rstock> Rstocks { get; set; }
    }
}
