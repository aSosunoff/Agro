using System;
using System.Collections.Generic;
using System.Linq;
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
        public static int GetValueInt<T>(this T item, string colName)
        {
            return (int)item.GetType().GetProperty(colName).GetValue(item, null);
        }
        public static decimal GetValueDecimal<T>(this T item, string colName)
        {
            return (decimal)item.GetType().GetProperty(colName).GetValue(item, null);
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
                    Value = System.Convert.ToString(el.ITEM.GetValueInt(valueColName))
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

        public static List<WrapModel<T>> ConnectByPriorAllElement<T>(this IEnumerable<T> list, Func<T, object> func)
        {
            if (list.Count() > 0)
            {
                var item = func.Invoke(list.ToList()[0]);
                var itemCount = item.GetType().GetProperties().Count();

                if ((itemCount >= 2) && (itemCount <= 3))
                {
                    string IdName = item.GetType().GetProperties()[0].Name;
                    string ParentIdName = item.GetType().GetProperties()[1].Name;

                    int RootVal = 0;
                    if (itemCount == 3)
                        RootVal = (int)item.GetType().GetProperties()[2].GetValue(item, null);

                    //TODO: пофиксить тип. сделать независимость к типу столбцов объектов
                    List<T> parentList = new List<T>();
                    switch (list.ToList()[0].GetType().GetProperty(ParentIdName).PropertyType.Name)
                    {
                        case "Decimal":
                            parentList = list.Where(e => e.GetValueDecimal(ParentIdName) == RootVal).ToList();
                            break;
                        case "Int32":
                            parentList = list.Where(e => e.GetValueInt(ParentIdName) == RootVal).ToList();
                            break;
                    }

                    List<WrapModel<T>> priorModels = new List<WrapModel<T>>();

                    foreach (var element in parentList)
                    {
                        switch (element.GetType().GetProperty(IdName).PropertyType.Name)
                        {
                            case "Decimal":
                                RootVal = (int)element.GetValueDecimal(IdName);
                                break;
                            case "Int32":
                                RootVal = element.GetValueInt(IdName);
                                break; 
                        }
                        

                        priorModels = list.ConnectByPrior(IdName, ParentIdName, RootVal, priorModels);
                    }
                    return priorModels;
                }
            }
            return null;
        }

        public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, Func<T, object> func, List<WrapModel<T>> priorModels = null)
        {
            if (list.Count() > 0)
            {
                var item = func.Invoke(list.ToList()[0]);
                var itemCount = item.GetType().GetProperties().Count();

                if ((itemCount >= 2) && (itemCount <= 3))
                {
                    string IdName = item.GetType().GetProperties()[0].Name;
                    string ParentIdName = item.GetType().GetProperties()[1].Name;

                    int RootVal = 0;
                    if (itemCount == 3)
                        RootVal = (int)item.GetType().GetProperties()[2].GetValue(item, null);

                    return list.ConnectByPrior(IdName, ParentIdName, RootVal, priorModels);
                }
            }
            return null;
        }

        private static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, string IdName, string ParentIdName, int RootVal, List<WrapModel<T>> priorModels = null)
        {
            if (priorModels == null)
                priorModels = new List<WrapModel<T>>();

            //Выбираем корневой элемент
            //TODO: жёстко привязано к Int32
            var currentElement = list.SingleOrDefault(e => e.GetValueInt(IdName) == RootVal);

            if (currentElement != null)
            {
                //Записываем индекс корневого элемента
                int lvl = (int)TheeLevel.Start;

                //Добавляем наш элемент в список который обёрнут в класс обёртку со своими полями данных
                priorModels.Add(new WrapModel<T>()
                {
                    ID = priorModels.Count + 1,
                    //Порядковый номер элемента
                    LEVEL = lvl,
                    //Уровень вложенности
                    ITEM = currentElement,
                    //Наш элемент
                    FLAG_TREE = true
                    //Флаг является ли элемент последним в цепочке
                });

                switch (currentElement.GetType().GetProperty(IdName).PropertyType.Name)
                {
                    case "Decimal":
                        RootVal = (int)currentElement.GetValueDecimal(IdName);
                        break;
                    case "Int32":
                        RootVal = currentElement.GetValueInt(IdName);
                        break;
                }

                //TODO: жёстко привязано к Int32
                if (list.Any(e => e.GetValueInt(ParentIdName) == RootVal))
                {
                    priorModels[priorModels.Count - 1].FLAG_TREE = false;
                    lvl++;
                    return ConnectByPriorLoop(list, IdName, ParentIdName, RootVal, priorModels, lvl);
                }
            }
            else
                return null;
            return priorModels;
        }

        private static List<WrapModel<T>> ConnectByPriorLoop<T>(IEnumerable<T> list, string IdName, string ParentIdName, int RootVal, List<WrapModel<T>> PriorModels, int LEVEL)
        {
            var elements = list.Where(e => e.GetValueInt(ParentIdName) == RootVal).ToList();

            for (int i = 0; i < elements.Count(); i++)
            {

                PriorModels.Add(new WrapModel<T>()
                {
                    ID = PriorModels.Count + 1,
                    LEVEL = LEVEL,
                    ITEM = elements[i],
                    FLAG_TREE = false
                });

                switch (elements[i].GetType().GetProperty(IdName).PropertyType.Name)
                {
                    case "Decimal":
                        RootVal = (int)elements[i].GetValueDecimal(IdName);
                        break;
                    case "Int32":
                        RootVal = elements[i].GetValueInt(IdName);
                        break;
                }

                if (list.Any(e => e.GetValueInt(ParentIdName) == RootVal))
                {
                    LEVEL += 1;
                    PriorModels = ConnectByPriorLoop(list, IdName, ParentIdName, RootVal, PriorModels, LEVEL);
                    LEVEL -= 1;
                }
                PriorModels[PriorModels.Count - 1].FLAG_TREE = true;
            }

            return PriorModels;
        }

        //public static List<WrapModel<T>> ConnectByPriorAllElement<T>(this IEnumerable<T> list, ConnectByPriorInModel model)
        //{
        //    var parentList = list.Where(e => e.GetValueInt(model.ConnectByPrior.Right) == model.StartWith.ColummValue).ToList();

        //    List<WrapModel<T>> priorModels = new List<WrapModel<T>>();

        //    foreach (var element in parentList)
        //    {
        //        model.StartWith.ColummValue = element.GetValueInt(model.ConnectByPrior.Left);

        //        priorModels = list.ConnectByPrior(model, priorModels);
        //    }
        //    return priorModels;
        //}

        //public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, ConnectByPriorInModel inModel, List<WrapModel<T>> priorModels = null)
        //{
        //    if (priorModels == null)
        //        priorModels = new List<WrapModel<T>>();

        //    var currentElement = list.SingleOrDefault(e => e.GetValueInt(inModel.StartWith.ColummName) == inModel.StartWith.ColummValue);
        //    //Выбираем корневой элемент
        //    if (currentElement != null)
        //    {
        //        //Записываем индекс корневого элемента
        //        int lvl = (int)TheeLevel.Start;

        //        //Добавляем наш элемент в список который обёрнут в класс обёртку со своими полями данных
        //        priorModels.Add(new WrapModel<T>()
        //        {
        //            ID = priorModels.Count + 1,
        //            //Порядковый номер элемента
        //            LEVEL = lvl,
        //            //Уровень вложенности
        //            ITEM = currentElement,
        //            //Наш элемент
        //            FLAG_TREE = true
        //            //Флаг является ли элемент последним в цепочке
        //        });

        //        inModel.StartWith.ColummValue = currentElement.GetValueInt(inModel.ConnectByPrior.Left);

        //        if (list.Any(e => e.GetValueInt(inModel.ConnectByPrior.Right) == inModel.StartWith.ColummValue))
        //        {
        //            priorModels[priorModels.Count - 1].FLAG_TREE = false;
        //            lvl++;
        //            return ConnectByPriorLoop(list, inModel, priorModels, lvl);
        //        }
        //    }
        //    else
        //        return null;
        //    return priorModels;
        //}

        //private static List<WrapModel<T>> ConnectByPriorLoop<T>(IEnumerable<T> list, ConnectByPriorInModel inModel, List<WrapModel<T>> PriorModels, int LEVEL)
        //{
        //    var elements = list.Where(e => e.GetValueInt(inModel.ConnectByPrior.Right) == inModel.StartWith.ColummValue).ToList();

        //    for (int i = 0; i < elements.Count(); i++)
        //    {

        //        PriorModels.Add(new WrapModel<T>()
        //        {
        //            ID = PriorModels.Count + 1,
        //            LEVEL = LEVEL,
        //            ITEM = elements[i],
        //            FLAG_TREE = false
        //        });

        //        inModel.StartWith.ColummValue = elements[i].GetValueInt(inModel.ConnectByPrior.Left);

        //        if (list.Any(e => e.GetValueInt(inModel.ConnectByPrior.Right) == inModel.StartWith.ColummValue))
        //        {
        //            LEVEL += 1;
        //            PriorModels = ConnectByPriorLoop(list, inModel, PriorModels, LEVEL);
        //            LEVEL -= 1;
        //        }
        //        PriorModels[PriorModels.Count - 1].FLAG_TREE = true;
        //    }

        //    return PriorModels;
        //}



        /////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////// 
    }
}