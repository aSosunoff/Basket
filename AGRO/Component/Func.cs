﻿using System;
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
        public decimal ColummValue { get; set; }
    }

    public class ConnectByPrior
    {
        public string Left { get; set; }
        public string Right { get; set; }
    }

    public static class JazzClass
    {
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



        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////


        public static List<WrapModel<T>> ConnectByPriorAllElement<T>(this IEnumerable<T> list, ConnectByPriorInModel model)
        {
            var parentList = list.Where(e => e.GetValueDecimal(model.ConnectByPrior.Right) == model.StartWith.ColummValue).ToList();

            List<WrapModel<T>> priorModels = new List<WrapModel<T>>();

            foreach (var element in parentList)
            {
                model.StartWith.ColummValue = element.GetValueDecimal(model.ConnectByPrior.Left);

                priorModels = list.ConnectByPrior(model, priorModels);
            }
            return priorModels;
        }


        public static List<WrapModel<T>> ConnectByPrior<T>(this IEnumerable<T> list, ConnectByPriorInModel inModel, List<WrapModel<T>> priorModels = null)
        {
            if(priorModels == null)
                priorModels = new List<WrapModel<T>>();

            var currentElement = list.SingleOrDefault(e => e.GetValueDecimal(inModel.StartWith.ColummName) == inModel.StartWith.ColummValue);
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

                inModel.StartWith.ColummValue = currentElement.GetValueDecimal(inModel.ConnectByPrior.Left);

                if (list.Any(e => e.GetValueDecimal(inModel.ConnectByPrior.Right) == inModel.StartWith.ColummValue))
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
            var elements = list.Where(e => e.GetValueDecimal(inModel.ConnectByPrior.Right) == inModel.StartWith.ColummValue).ToList();

            for (int i = 0; i < elements.Count(); i++)
            {

                PriorModels.Add(new WrapModel<T>()
                {
                    ID = PriorModels.Count + 1,
                    LEVEL = LEVEL,
                    ITEM = elements[i],
                    FLAG_TREE = false
                });

                inModel.StartWith.ColummValue = elements[i].GetValueDecimal(inModel.ConnectByPrior.Left);

                if (list.Any(e => e.GetValueDecimal(inModel.ConnectByPrior.Right) == inModel.StartWith.ColummValue))
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
    }
}