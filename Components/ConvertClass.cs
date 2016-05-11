using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Components
{
    public static class ConvertClass
    {
        /// <summary>
        /// Возвращаем базовый тип, если пришёл тип Nullable
        /// </summary>
        private static Type GetCoreType(Type t)
        {
            if ((t != null) && t.IsValueType && t.IsGenericType)
            {
                if (t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            return t;
        }

        public static DataTable ToDataTable<T>(this T item, string nameDataTable, Func<T, object> value)
        {
            return ToDataTable(nameDataTable, value.Invoke(item));
        }

        public static DataTable ToDataTable(string nameDataTable, object value)
        {
            var propInfo = value.GetType().GetProperties();

            var dt = new DataTable(nameDataTable);

            dt.Columns.AddRange(
                    propInfo.Select(e => new DataColumn(e.Name, GetCoreType(e.PropertyType))).ToArray()
                );

            dt.Rows.Add(propInfo.Select((e, i) => value.GetType().GetProperty(propInfo[i].Name).GetValue(value, null)).ToArray());

            return dt;
        }


        public static DataTable ToDataTable<T>(this IEnumerable<T> items, string nameDataTable, Expression<Func<T, object>> value)
        {
            List<object> source = new List<object>();

            items.ToList().ForEach(
                    i => source.Add(value.Compile().Invoke(i))
                );

            var dt = new DataTable(nameDataTable);
            //Members - используем для того что бы достать имя
            var Members = ((NewExpression)value.Body).Members;//nameCol
            //Arguments - используем для того что бы достать тип
            var Arguments = ((NewExpression)value.Body).Arguments;//typeCol
            //TODO: попытаться использовать что то одно. Из Members попробовать Type что бы не использовать вспомогательную строку с Arguments
            for (int i = 0; i < Members.Count; i++)
                dt.Columns.Add(new DataColumn(Members[i].Name, GetCoreType(Arguments[i].Type)));

            if (source.Count() > 0)
            {
                var prorpInfo = source.ToList()[0].GetType().GetProperties();

                source.ToList().ForEach(
                    i => dt.Rows.Add(prorpInfo.Select(p => p.GetValue(i, null)).ToArray())
                    );
            }
            return dt;
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
