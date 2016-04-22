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
using Model.Engine.Repository;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AGRO.Controllers
{
    public class ProductController : Controller
    {
        public IServiceLayer _serviceLayer { get; set; }
        private StartViewBag StartViewBag { get; set; }

        public ProductController(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);

            StartViewBag = new StartViewBag(_serviceLayer);

            ViewBag.CountElementToBasket = StartViewBag.CountElementToBasket;
            ViewBag.CountElementToContract = StartViewBag.CountElementToContract;
            ViewBag.WrapModels = StartViewBag.WrapModels;


        }

        public ActionResult Category(int id)
        {
            ModelCatalog modelCatalog = new ModelCatalog();

            ConnectByPriorInModel model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "ID",
                    ColummValue = id
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "ID",
                    Right = "P_ID"
                }
            };
            //TODO: Сделать Reset модели 

            modelCatalog.Categorys = _serviceLayer
                .Get<ICategoryService>()
                                        ._Repository
                                        .GetAllList()
                                        .ConnectByPrior(model)
                                        .Where(e => e.LEVEL > 1)
                                        .RemoveWrapModel();


            model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "ID",
                    ColummValue = id
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "ID",
                    Right = "P_ID"
                }
            };


            decimal[] arrayIdCategory = _serviceLayer
                .Get<ICategoryService>()
                ._Repository
                .GetAllList()
                .ConnectByPrior(model)
                .Where(e => e.FLAG_TREE)
                .RemoveWrapModel().Select(e => e.ID)
                .ToArray();

            modelCatalog.Products = _serviceLayer
                .Get<IProductService>()
                ._Repository
                .GetSortList(e => arrayIdCategory
                    .Contains(e.ID_CATEGORY))
                .ToList();

            return View(modelCatalog);
        }

        public ActionResult Index()
        {
            //MyClass myClass = new MyClass(new ServiceLayer(new UnitOfWork()));
            

            //ConnectByPriorInModel model = new ConnectByPriorInModel()
            //{
            //    StartWith = new StartWith()
            //    {
            //        ColummName = "ID",
            //        ColummValue = id
            //    },
            //    ConnectByPrior = new ConnectByPrior()
            //    {
            //        Left = "ID",
            //        Right = "P_ID"
            //    }
            //};

            //var f = _serviceLayer.Get<ITestService>().TestRepository.GetAllList().ConnectByPriorAllElement(model).Where(e => e.FLAG_TREE);

          return View(_serviceLayer.Get<IProductService>()._Repository.GetAllList());
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
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View("BasketAdd", basket);
        }

        public ActionResult Basket()
        {
            try
            {
                //Необходимо дклать проверку перед открытием корзины
                //todo: Необходимо сделать всё автоматом. Проблемма в том что когда отдаётся модель во view начитает работать процедура ProductsToBascet, которая выкидывает ошибку. Но так как модель уже находиться во View она не может её вернуть в контроллер где эта ошибка возвращается
                //
                StartViewBag.IsProductsToBascet();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(_serviceLayer.Get<IBasketService>()._Repository.GetAllList());
        }

        public ActionResult OrderList()
        {
            return View(_serviceLayer.Get<IContractService>().GetList());
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
                ViewBag.ErrorMessage = ex.Message;
            }
            return View("Basket", _serviceLayer.Get<IBasketService>()._Repository.GetAllList());
            //todo: Вывести детальное предупреждение. Предусмотреть удаление товара из корзины которого нет на складе. Так же итоговую сумму выводить без сложения с товаром которого нет на складе
        }

        

        public ActionResult Order(decimal id)
        {
            return View("OrderDetails", _serviceLayer.Get<IOrderService>().GetOrdersByIdContract(id));
        }

    }
}
