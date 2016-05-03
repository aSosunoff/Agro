using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public static class Convert
    {
        public static DataTable ConvertToDataTable(string nameDataTable, object value)
        {
            var propInfo = value.GetType().GetProperties();

            var dt = new DataTable(nameDataTable);

            dt.Columns.AddRange(
                    propInfo.Select(e => new DataColumn(e.Name, e.PropertyType)).ToArray()
                );

            dt.Rows.Add(propInfo.Select((e, i) => value.GetType().GetProperty(propInfo[i].Name).GetValue(value, null)).ToArray());

            return dt;
        }

        public static DataTable ConvertToDataTable(string nameDataTable, IEnumerable<object> source)
        {
            if (source.Count() > 0)
            {
                var prorpInfo = source.ToList()[0].GetType().GetProperties();

                var dt = new DataTable(nameDataTable);

                dt.Columns.AddRange(
                  prorpInfo.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
                );

                source.ToList().ForEach(
                  i => dt.Rows.Add(prorpInfo.Select(p => p.GetValue(i, null)).ToArray())
                );

                return dt;
            }
            else
            {
                throw new Exception("Is not found element");
            }  
        }

        ///// <summary>
        ///// Convert a List{T} to a DataTable.
        ///// </summary>
        //public static DataTable ToDataTable<T>(this List<T> items)
        //{
        //    var tb = new DataTable(typeof(T).Name);

        //    PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (PropertyInfo prop in props)
        //    {
        //        Type t = GetCoreType(prop.PropertyType);
        //        tb.Columns.Add(prop.Name, t);
        //    }


        //    foreach (T item in items)
        //    {
        //        var values = new object[props.Length];

        //        for (int i = 0; i < props.Length; i++)
        //        {
        //            values[i] = props[i].GetValue(item, null);
        //        }

        //        tb.Rows.Add(values);
        //    }

        //    return tb;
        //}

        ///// <summary>
        ///// Determine of specified type is nullable
        ///// </summary>
        //public static bool IsNullable(Type t)
        //{
        //    return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        //}

        ///// <summary>
        ///// Return underlying type if type is Nullable otherwise return the type
        ///// </summary>
        //public static Type GetCoreType(Type t)
        //{
        //    if (t != null && IsNullable(t))
        //    {
        //        if (!t.IsValueType)
        //        {
        //            return t;
        //        }
        //        else
        //        {
        //            return Nullable.GetUnderlyingType(t);
        //        }
        //    }
        //    else
        //    {
        //        return t;
        //    }
        //}
    }
}
