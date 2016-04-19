using System;
using System.Collections.Generic;
using System.Linq;

namespace AgroFirma.Component
{
    public class ConnectByPriorModel<T>
    {
        public int ID_MODEL { get; set; }
        public int LEVEL { get; set; }
        public T ITEM { get; set; }
    }

    public class ConnectByPriorModelSort<T> : ConnectByPriorModel<T>
    {
        public List<ConnectByPriorModel<T>> List;
    }

    public static class JazzClass
    {
        private static string _startWithPropertyName = "ID";

        private static string _byPriorName = "P_ID";


        //public static ConnectByPriorModelSort<T> Pac<T>(this List<ConnectByPriorModel<T>> list)
        //{
        //    var maxLevel = list.Max(e => e.LEVEL);

        //}






        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, string startWithPropertyName, string byPriorName, int startWithId, List<ConnectByPriorModel<T>> PriorModels)
        {
            _startWithPropertyName = startWithPropertyName;
            _byPriorName = byPriorName;

            return ConnectByPriorDown(list, startWithId, PriorModels);
        }

        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, string startWithPropertyName, string byPriorName, int startWithId)
        {
            _startWithPropertyName = startWithPropertyName;
            _byPriorName = byPriorName;

            return ConnectByPriorDown(list, startWithId, null);
        }



        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, int startWithId)
        {
            return ConnectByPriorDown(list, startWithId, null);
        }

        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, int startWithId, List<ConnectByPriorModel<T>> PriorModels)
        {
            return ConnectByPriorDown(list, startWithId, PriorModels);
        }







        public static List<ConnectByPriorModel<T>> ConnectByPriorAllElement<T>(this IEnumerable<T> list, int perentId, string parentNameCol, List<ConnectByPriorModel<T>> priorModels = null, int i = 0)
        {
            parentNameCol = "PARENT_ID";

            var parentList = list.Where(e => (int)e.GetType().GetProperty(parentNameCol).GetValue(e, null) == perentId).ToList();


            foreach (var element in parentList)
            {
                if (priorModels == null)
                    priorModels = new List<ConnectByPriorModel<T>>();
                priorModels = list.ConnectByPrior("PK_ID", "PARENT_ID", (int)element.GetType().GetProperty("PK_ID").GetValue(element, null), priorModels);
            }
            return priorModels;
        }



















        private enum TheeLevel
        {
            Start = 1
        }

        private static List<ConnectByPriorModel<T>> ConnectByPriorDown<T>(IEnumerable<T> list, int startWithId, List<ConnectByPriorModel<T>> priorModels, int level = (int)TheeLevel.Start)
        {
            var currentElement = list.SingleOrDefault(e => (int)e.GetType().GetProperty(_startWithPropertyName).GetValue(e, null) == startWithId);
            if (currentElement != null)
            {
                if (level == (int)TheeLevel.Start)
                {
                    //TODO: повторение кода
                    if (priorModels == null)
                        priorModels = new List<ConnectByPriorModel<T>>();

                    priorModels.Add(new ConnectByPriorModel<T>()
                    {
                        ID_MODEL = priorModels.Count + 1,
                        LEVEL = level,
                        ITEM = currentElement
                    });

                    level++;
                }

                var elements = list.Where(e => (int)e.GetType().GetProperty(_byPriorName).GetValue(e, null) == startWithId).ToList();

                for (int i = 0; i < elements.Count(); i++)
                {

                    priorModels.Add(new ConnectByPriorModel<T>()
                    {
                        ID_MODEL = priorModels.Count + 1,
                        LEVEL = level,
                        ITEM = elements[i]
                    });

                    startWithId = (int)elements[i].GetType().GetProperty(_startWithPropertyName).GetValue(elements[i], null);

                    if (list.Any(e => (int)e.GetType().GetProperty(_byPriorName).GetValue(e, null) == startWithId))
                    {
                        level += 1;
                        priorModels = ConnectByPriorDown(list, startWithId, priorModels, level);
                        level -= 1;
                    }
                }

                return priorModels;
            }

            throw new Exception("Error");
        }
    }
}