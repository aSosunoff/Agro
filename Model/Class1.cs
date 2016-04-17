using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Class1
    {
        public void Add()
        {
            agro_mydbEntities d = new agro_mydbEntities();

            rnews n = new rnews()
            {
                DATE = DateTime.Now,
                IMAGE_PATH = "asdas",
                NAME = "sadfgg",
                TEXT = "asddasd"
            };

            d.Entry(n).State = EntityState.Added;

            d.SaveChanges();
        }
    }
}
