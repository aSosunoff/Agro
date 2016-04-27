using System.Collections.Generic;
using Model;

namespace AgroFirma.Component
{
    public class CategoryModel
    {
        public IEnumerable<ccategory> Ccategories { get; set; }
        public IEnumerable<rstock> Rstocks { get; set; }
    }
}
