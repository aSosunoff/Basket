using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Engine.Service.Interface;
using System.Reflection;

namespace AGRO.Component
{
    public class ConnectByPriorModel<T>
    {
        public int ID_MODEL { get; set; }
        public int LEVEL { get; set; }
        public T ITEM { get; set; }
    }
    public static class JazzClass
    {
        private static string StartWithPropertyName = "ID";

        private static string ByPriorName = "P_ID";


        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, string strWithPropertyName, object value, string byPriorName)
        {
            return ConnectByPriorDown(list, startWithId, null, (int)TheeLevel.Start);
        }




        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, decimal startWithId)
        {
            return ConnectByPriorDown(list, startWithId, null);
        }

        public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, decimal startWithId, List<ConnectByPriorModel<T>> PriorModels)
        {
            return ConnectByPriorDown(list, startWithId, PriorModels);
        }







        public static List<ConnectByPriorModel<T>> ConnectByPriorAllElement<T>(this IEnumerable<T> list, decimal startWithId, List<ConnectByPriorModel<T>> PriorModels)
        {
            return ConnectByPriorDown(list, startWithId, PriorModels);
        }



        private enum TheeLevel
        {
            Start = 1
        }

        private static List<ConnectByPriorModel<T>> ConnectByPriorDown<T>(IEnumerable<T> list, decimal startWithId, List<ConnectByPriorModel<T>> PriorModels, int LEVEL = (int)TheeLevel.Start)
        {
            var currentElement = list.SingleOrDefault(e => (decimal)e.GetType().GetProperty(StartWithPropertyName).GetValue(e, null) == startWithId);
            if (currentElement != null)
            {
                if (LEVEL == (int) TheeLevel.Start)
                {
                    //TODO: повторение кода
                    if (PriorModels == null)
                        PriorModels = new List<ConnectByPriorModel<T>>();

                    PriorModels.Add(new ConnectByPriorModel<T>()
                    {
                        ID_MODEL = PriorModels.Count + 1,
                        LEVEL = LEVEL,
                        ITEM = currentElement
                    });

                    LEVEL++;
                }

                var elements = list.Where(e => (decimal)e.GetType().GetProperty(ByPriorName).GetValue(e, null) == startWithId).ToList();

                for (int i = 0; i < elements.Count(); i++)
                {
                    if (PriorModels == null)
                        PriorModels = new List<ConnectByPriorModel<T>>();

                    PriorModels.Add(new ConnectByPriorModel<T>()
                    {
                        ID_MODEL = PriorModels.Count + 1,
                        LEVEL = LEVEL,
                        ITEM = elements[i]
                    });

                    startWithId = (decimal)elements[i].GetType().GetProperty(StartWithPropertyName).GetValue(elements[i], null);

                    if (list.Any(e => (decimal)e.GetType().GetProperty(ByPriorName).GetValue(e, null) == startWithId))
                    {
                        LEVEL += 1;
                        PriorModels = ConnectByPriorDown(list, startWithId, PriorModels, LEVEL);
                        LEVEL -= 1;
                    }
                }

                return PriorModels;
            }

            throw new Exception("Error"); 
        }

        //public static List<ConnectByPriorModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, List<ConnectByPriorModel<T>> PriorModels = null, decimal parent = 0)
        //{
        //    //Type type = typeof(T);
        //    //foreach (T VARIABLE in list)
        //    //{
        //    //    Type e = VARIABLE.GetType();
        //    //    object value = e.GetProperty("ID").GetValue(VARIABLE, null);
        //    //}

        //    var elements = list.Where(e => (decimal)e.GetType().GetProperty("P_ID").GetValue(e, null) == parent).ToList();


        //    for (int i = 0; i < elements.Count(); i++)
        //    {
        //        decimal idItem = (decimal)elements[i].GetType().GetProperty("ID").GetValue(elements[i], null);

        //        if (list.Any(e => (decimal) e.GetType().GetProperty("P_ID").GetValue(e, null) == idItem))
        //            list.ConnectByPrior(PriorModels, (decimal)elements[i].GetType().GetProperty("ID").GetValue(elements[i], null));

        //        if (PriorModels == null)
        //            PriorModels = new List<ConnectByPriorModel<T>>();

        //        PriorModels.Add(new ConnectByPriorModel<T>()
        //        {
        //            ID_MODEL = PriorModels.Count,
        //            LEVEL = i,
        //            ITEM = elements[i]
        //        });
        //    }

        //    return PriorModels;
        //}
    }
}