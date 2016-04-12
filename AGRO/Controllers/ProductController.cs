using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGRO.Models;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using AGRO_BASKET = Model.AGRO_BASKET;

namespace AGRO.Controllers
{
    public class ProductController : Controller
    {
        private IServiceLayer _serviceLayer { get; set; }
        public ProductController(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);
        }
        public ActionResult Index()
        {
            return View(_serviceLayer.Get<IProductService>().GetBasketModels());
        }

        public ActionResult Details(int id)
        {
            return View(_serviceLayer.Get<IProductService>().GetItemToId(id));
        }

        public ActionResult AddedToBasket(int id)
        {
            AGRO_BASKET basket = new AGRO_BASKET()
            {
                ID_PRODUCT = id
            };

            return View("BasketAdd", basket);
        }

        [HttpPost]
        public ActionResult AddedToBasket(AGRO_BASKET product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _serviceLayer.Get<IBasketService>().AddedProductToBasket(product);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState["QANTITY"].Errors.Add(ex.Message);
                }
            }

            return View("BasketAdd", product);
        }

        public ActionResult Basket()
        {
            return View(_serviceLayer.Get<IBasketService>().GetBasketModels());
        }

        public ActionResult BasketToOrder()
        {
            _serviceLayer.Get<IBasketService>().Order();
            return RedirectToAction("Index");
        }

        public ActionResult OrderList()
        {
            return View(_serviceLayer.Get<IContractService>().GetList());
        }

        public ActionResult Order(decimal id)
        {
            return View("OrderDetails", _serviceLayer.Get<IOrderService>().GetOrdersByIdContract(id));
        }

    }
}
