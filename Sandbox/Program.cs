using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace Sandbox
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

    static class Program
    {
        private enum TheeLevel
        {
            Start = 1
        }

        static void Main(string[] args)
        {
            ServiceLayer serviceLayer = new ServiceLayer(new UnitOfWork());

            IServiceLayer _serviceLayer = ServiceLayer.Instance(serviceLayer);

            var wrapModelList = _serviceLayer
                .Get<ICCategoryService>()
                ._Repository
                .GetAllList()
                .ConnectByPrior(
                e =>
                    new
                    {
                        e.PK_ID,
                        e.PARENT_ID,
                        ROOT = 666
                    });
        }

        private static int getValue<T>(this T t, string idName)
        {
            int RootVal = 0;

            switch (t.GetType().GetProperty(idName).PropertyType.Name)
            {
                case "Decimal":
                    RootVal = (int)(decimal)t.GetType().GetProperty(idName).GetValue(t, null);
                    break;
                case "Int32":
                    RootVal = (int)t.GetType().GetProperty(idName).GetValue(t, null);
                    break;
            }

            return RootVal;
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
                        RootVal = (int)((ConstantExpression)((NewExpression)funcEx.Body).Arguments[2]).Value;

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
