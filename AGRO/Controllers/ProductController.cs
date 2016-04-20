using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGRO.Component;
using AGRO.Models;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AGRO.Controllers
{
    public class ProductController : Controller
    {
        private IServiceLayer _serviceLayer { get; set; }
        private PageModels PageModels { get; set; }
        public ProductController(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);

            PageModels = new PageModels(_serviceLayer);
        }

        //class ModelDown
        //{
        //    public int ID_MODEL_DOWN { get; set; }
        //    public int LEVEL { get; set; }
        //    public AGRO_TEST AgroTest { get; set; }
        //}

        //private List<ModelDown> Down(List<ModelDown> modelDowns, decimal parent = 0)
        //{
        //    var elements = _serviceLayer.Get<ITestService>().TestRepository.GetSortList(e => e.P_ID == parent).ToList();

        //    for (int i = 0; i < elements.Count(); i++)
        //    {
        //        decimal idItem = elements[i].ID;
        //        if (_serviceLayer.Get<ITestService>().TestRepository.GetSortList(e => e.P_ID == idItem).Any())
        //            Down(modelDowns, elements[i].ID);

        //        modelDowns.Add(new ModelDown()
        //        {
        //            ID_MODEL_DOWN = modelDowns.Count,
        //            LEVEL = i,
        //            AgroTest = elements[i]
        //        });
        //    }
        //    return modelDowns;
        //}

        

        public ActionResult Index()
        {
            //ConnectByPriorInModel model = new ConnectByPriorInModel()
            //{
            //    StartWith = new StartWith()
            //    {
            //        ColummName = "ID",
            //        ColummValue = 16
            //    },
            //    ConnectByPrior = new ConnectByPrior()
            //    {
            //        Left = "ID",
            //        Right = "P_ID"
            //    }
            //};

            //var f = _serviceLayer.Get<ITestService>().TestRepository.GetAllList().ConnectByPrior(model);

            ConnectByPriorInModel model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "ID",
                    ColummValue = 0
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "ID",
                    Right = "P_ID"
                }
            };

            var f = _serviceLayer.Get<ITestService>().TestRepository.GetAllList().ConnectByPriorAllElement(model).Where(e => e.FLAG_TREE == true);

            return View(PageModels);
        }

        public ActionResult Details(int id)
        {
            PageModels.Product = _serviceLayer.Get<IProductService>().GetItemToId(id);

            return View(PageModels);
        }

        public ActionResult AddedToBasket(int id)
        {
            AGRO_BASKET basket = new AGRO_BASKET()
            {
                ID_PRODUCT = id
            };
            PageModels.Basket = basket;

            return View("BasketAdd", PageModels);
        }

        [HttpPost]
        public ActionResult AddedToBasket(AGRO_BASKET basket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _serviceLayer.Get<IBasketService>().AddedProductToBasket(basket);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState["basket.QANTITY"].Errors.Add(ex.Message);
                    PageModels.ErrorMessage = ex.Message;
                }
            }
            PageModels.Basket = basket;
            return View("BasketAdd", PageModels);
        }

        public ActionResult Basket()
        {
            try
            {
                //Необходимо дклать проверку перед открытием корзины
                //todo: Необходимо сделать всё автоматом. Проблемма в том что когда отдаётся модель во view начитает работать процедура ProductsToBascet, которая выкидывает ошибку. Но так как модель уже находиться во View она не может её вернуть в контроллер где эта ошибка возвращается
                //
                PageModels.IsProductsToBascet();
            }
            catch (Exception ex)
            {
                PageModels.ErrorMessage = ex.Message;
            }
            return View(PageModels);
        }

        public ActionResult OrderList()
        {
            return View(PageModels);
        }
        
        public ActionResult BasketToOrder()
        {
            try
            {
                _serviceLayer.Get<IBasketService>().Order();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                PageModels.ErrorMessage = ex.Message;
            }
            return View("Basket", PageModels);
            //todo: Вывести детальное предупреждение. Предусмотреть удаление товара из корзины которого нет на складе. Так же итоговую сумму выводить без сложения с товаром которого нет на складе
        }

        

        public ActionResult Order(decimal id)
        {
            PageModels.Orders = _serviceLayer.Get<IOrderService>().GetOrdersByIdContract(id);
            return View("OrderDetails", PageModels);
        }

    }
}
