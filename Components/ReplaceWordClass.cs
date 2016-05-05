using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace Components
{
    public static class ReplaceWordClass
    {
        public static void ReplaseWordStub<T>(this T item, Func<T, object> func, DocumentClass wordDocument)
        {
            var value = func.Invoke(item);

            var valueProperties = value.GetType().GetProperties().ToList();

            for (int i = 0; i < valueProperties.Count; i++)
            {
                wordDocument.Content.Find.ClearFormatting();

                wordDocument.Content
                    .Find
                    .Execute(
                        FindText: String.Format("<{0}>", valueProperties[i].Name),
                        ReplaceWith: Convert.ToString(valueProperties[i].GetValue(value, null))
                    );
            }
        }
    }
}
