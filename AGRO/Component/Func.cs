using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Engine.Service.Interface;
using System.Reflection;

namespace AGRO.Component
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

    public class ConnectByPriorInModel
    {
        public StartWith StartWith { get; set; }
        public ConnectByPrior ConnectByPrior { get; set; }
    }

    public class StartWith
    {
        public string ColummName { get; set; }
        //todo поменять на независимый тип. сейчас вытягивается поле из бд которое имеет тип Decimal
        public decimal ColummValue { get; set; }
    }

    public class ConnectByPrior
    {
        public string Left { get; set; }
        public string Right { get; set; }
    }








    public static class JazzClass
    {
        private static string StartWithPropertyName = "ID";

        private static string ByPriorName = "P_ID";



        public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, decimal startWithId)
        {
            return ConnectByPriorDown(list, startWithId, null);
        }

        public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, decimal startWithId, List<WrapModel<T>> PriorModels)
        {
            return ConnectByPriorDown(list, startWithId, PriorModels);
        }








        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////


        public static List<WrapModel<T>> ConnectByPriorAllElement<T>(this IEnumerable<T> list, ConnectByPriorInModel model)
        {
            var parentList = list.Where(e => (decimal)e.GetType().GetProperty(model.ConnectByPrior.Right).GetValue(e, null) == model.StartWith.ColummValue).ToList();

            List<WrapModel<T>> priorModels = new List<WrapModel<T>>();

            foreach (var element in parentList)
            {
                model.StartWith.ColummValue = (decimal)element.GetType().GetProperty(model.ConnectByPrior.Left).GetValue(element, null);

                priorModels = list.ConnectByPrior(model, priorModels);
            }
            return priorModels;
        }


        public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, ConnectByPriorInModel inModel, List<WrapModel<T>> priorModels = null)
        {
            if(priorModels == null)
                priorModels = new List<WrapModel<T>>();

            var currentElement = list.SingleOrDefault(e => (decimal)e.GetType().GetProperty(inModel.StartWith.ColummName).GetValue(e, null) == inModel.StartWith.ColummValue);
            //Выбираем корневой элемент
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

                inModel.StartWith.ColummValue = (decimal)currentElement.GetType().GetProperty(inModel.ConnectByPrior.Left).GetValue(currentElement, null);

                if (list.Any(e => (decimal)e.GetType().GetProperty(inModel.ConnectByPrior.Right).GetValue(e, null) == inModel.StartWith.ColummValue))
                {
                    priorModels[priorModels.Count - 1].FLAG_TREE = false;
                    lvl++;
                    return ConnectByPriorLoop(list, inModel, priorModels, lvl);
                }
            }
            else
                return null;
            return priorModels;
        }

        private static List<WrapModel<T>> ConnectByPriorLoop<T>(IEnumerable<T> list, ConnectByPriorInModel inModel, List<WrapModel<T>> PriorModels, int LEVEL)
        {
            var elements = list.Where(e => (decimal)e.GetType().GetProperty(inModel.ConnectByPrior.Right).GetValue(e, null) == inModel.StartWith.ColummValue).ToList();

            for (int i = 0; i < elements.Count(); i++)
            {

                PriorModels.Add(new WrapModel<T>()
                {
                    ID = PriorModels.Count + 1,
                    LEVEL = LEVEL,
                    ITEM = elements[i],
                    FLAG_TREE = false
                });

                inModel.StartWith.ColummValue = (decimal)elements[i].GetType().GetProperty(inModel.ConnectByPrior.Left).GetValue(elements[i], null);

                if (list.Any(e => (decimal)e.GetType().GetProperty(inModel.ConnectByPrior.Right).GetValue(e, null) == inModel.StartWith.ColummValue))
                {
                    LEVEL += 1;
                    PriorModels = ConnectByPriorLoop(list, inModel, PriorModels, LEVEL);
                    LEVEL -= 1;
                }
                PriorModels[PriorModels.Count - 1].FLAG_TREE = true;
            }

            return PriorModels;
        }



        /////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////// 

        private enum TheeLevel
        {
            Start = 1
        }

        private static List<WrapModel<T>> ConnectByPriorDown<T>(IEnumerable<T> list, decimal startWithId, List<WrapModel<T>> PriorModels, int LEVEL = (int)TheeLevel.Start)
        {
            var currentElement = list.SingleOrDefault(e => (decimal)e.GetType().GetProperty(StartWithPropertyName).GetValue(e, null) == startWithId);
            if (currentElement != null)
            {
                if (LEVEL == (int) TheeLevel.Start)
                {
                    //TODO: повторение кода
                    if (PriorModels == null)
                        PriorModels = new List<WrapModel<T>>();

                    PriorModels.Add(new WrapModel<T>()
                    {
                        ID = PriorModels.Count + 1,
                        LEVEL = LEVEL,
                        ITEM = currentElement,
                        FLAG_TREE = false
                    });

                    LEVEL++;
                }

                var elements = list.Where(e => (decimal)e.GetType().GetProperty(ByPriorName).GetValue(e, null) == startWithId).ToList();

                for (int i = 0; i < elements.Count(); i++)
                {

                    PriorModels.Add(new WrapModel<T>()
                    {
                        ID = PriorModels.Count + 1,
                        LEVEL = LEVEL,
                        ITEM = elements[i],
                        FLAG_TREE = false
                    });

                    startWithId = (decimal)elements[i].GetType().GetProperty(StartWithPropertyName).GetValue(elements[i], null);

                    if (list.Any(e => (decimal) e.GetType().GetProperty(ByPriorName).GetValue(e, null) == startWithId))
                    {
                        LEVEL += 1;
                        PriorModels = ConnectByPriorDown(list, startWithId, PriorModels, LEVEL);
                        LEVEL -= 1;
                    }
                    PriorModels[PriorModels.Count - 1].FLAG_TREE = true;
                }

                return PriorModels;
            }

            throw new Exception("Error"); 
        }
    }
}