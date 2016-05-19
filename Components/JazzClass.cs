using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Components
{
    public class WrapModel<T>
    {
        public int ID { get; set; }
        //идентификатор элемента дерева в модели
        public int LEVEL { get; set; }
        //уровень вложенности
        public bool FLAG_TREE { get; set; }
        //является ли элемент корнем дерева
        public T ITEM { get; set; }
        //элемент дерева
    }

    public static class JazzClass
    {           
        
        /////////////////////////////////////////////////////////////////////////////////////////
        public static string GetValueString<T>(this T item, string colName)
        {
            return (string)item.GetType().GetProperty(colName).GetValue(item, null);
        }


        public static IEnumerable<T> RemoveWrapModel<T>(this IEnumerable<WrapModel<T>> wrapModels)
        {
            List<T> list = new List<T>();

            foreach (var element in wrapModels)
            {
                list.Add(element.ITEM);
            }
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////////
        public static IEnumerable<SelectListItem> SelectListItemsAddedFirstItem(this IEnumerable<SelectListItem> selectListItems, string value, string text)
        {

            List<SelectListItem> listItems = new List<SelectListItem>();

            listItems.Add(new SelectListItem()
            {
                Text = text,
                Value = value
            });

            foreach (var el in selectListItems)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = el.Text,
                    Value = el.Value
                });
            }
            return listItems;
        }

        public static IEnumerable<SelectListItem> PackSelectListItem<T>(this IEnumerable<WrapModel<T>> list, string valueColName, string textColName)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (var el in list)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = el.ITEM.GetValueString(textColName),
                    Value = Convert.ToString(el.ITEM.getValue(valueColName))
                });
            }
            return listItems;
        }
        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        private enum TheeLevel
        {
            Start = 1
        }

        public static int getValue<T>(this T t, string idName)
        {
            int val = 0;

            switch (t.GetType().GetProperty(idName).PropertyType.Name)
            {
                case "Decimal":
                    val = (int)(decimal)t.GetType().GetProperty(idName).GetValue(t, null);
                    break;
                case "Int32":
                    val = (int)t.GetType().GetProperty(idName).GetValue(t, null);
                    break;
            }

            return val;
        }

        public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, Expression<Func<T, object>> funcEx)
        {
            
            if (list.Count() > 0)
            {
                //Получаем колличество аргументов их должно быть 2 - 3. первые 2 это имена id и parentId, 3 это id этолемента с которого начинается построение дерева
                var argCount = ((NewExpression)funcEx.Body).Arguments.Count;

                if ((argCount >= 2) && (argCount <= 3))
                {
                    //Достаём имя столбца с id
                    //Достаём имя столбца с parentId
                    string idName = ((NewExpression)funcEx.Body).Members[0].Name;
                    string ParentIdName = ((NewExpression)funcEx.Body).Members[1].Name;

                    int RootVal = 0;
                    if (argCount == 3)
                    { 
                        switch (((NewExpression)funcEx.Body).Arguments[2].GetType().FullName.ToString())
                        {
                            case "System.Linq.Expressions.ConstantExpression":
                                RootVal = 
                                    (int)
                                    ((ConstantExpression)
                                        ((NewExpression)funcEx.Body)
                                            .Arguments[2])
                                                .Value;
                                break;
                            case "System.Linq.Expressions.FieldExpression":
                                var argField = (MemberExpression) ((NewExpression) funcEx.Body).Arguments[2];
                                RootVal =
                                (int)
                                ((ConstantExpression)
                                    argField.Expression).Value
                                    .GetType()
                                    .GetField(argField.Member.Name)
                                    .GetValue(
                                        ((ConstantExpression)
                                            argField.Expression).Value);
                                break;
                            case "System.Linq.Expressions.PropertyExpression":
                                //((ConstantExpression)((MemberExpression)argProperty.Expression).Expression).Value.GetType().GetField(((MemberExpression)argProperty.Expression).Member.Name)
                                //TODO: пофиксить не работает как надо
                                var argProperty = (MemberExpression)((NewExpression) funcEx.Body).Arguments[2];
                                RootVal =
                                (int)
                                ((ConstantExpression)
                                    ((MemberExpression)
                                        argProperty.Expression).Expression).Value
                                        .GetType()
                                        .GetField(argProperty.Member.Name)
                                        .GetValue(
                                            ((ConstantExpression)
                                                argProperty.Expression).Value);
                                break;
                        }
                    }

                        

                    return loop(list, idName, ParentIdName, RootVal);
                }
            }
            return null;
        }

        private static List<WrapModel<T>> loop<T>(IEnumerable<T> list, string idName, string ParentIdName, int RootVal, List<WrapModel<T>> priorModels = null, int lvl = (int)TheeLevel.Start)
        {
            //Достаём родителей первого уровня
            List<T> parentList = list.Where(e => e.getValue(ParentIdName) == RootVal).ToList();

            if (priorModels == null)
                priorModels = new List<WrapModel<T>>();

            foreach (var element in parentList)
            {
                //Добавляем наш элемент в список который обёрнут в класс обёртку со своими полями данных
                priorModels.Add(new WrapModel<T>()
                {
                    ID = priorModels.Count + 1,
                    //Порядковый номер элемента
                    LEVEL = lvl,
                    //Уровень вложенности
                    ITEM = element,
                    //Наш элемент
                    FLAG_TREE = true
                    //Флаг является ли элемент последним в цепочке
                });

                RootVal = element.getValue(idName);

                if (list.Any(e => e.getValue(ParentIdName) == RootVal))
                {
                    priorModels[priorModels.Count - 1].FLAG_TREE = false;
                    lvl++;
                    priorModels = loop(list, idName, ParentIdName, RootVal, priorModels, lvl);
                    lvl--;
                }
            }
            return priorModels;
        }
    }
}