using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Engine.Service;

namespace AGRO.Component
{
    public class MainController : Controller
    {
        public IServiceLayer _serviceLayer { get; set; }
        public StartViewBag StartViewBag { get; set; }

        public MainController(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);

            StartViewBag = new StartViewBag(_serviceLayer);

            ViewBag.CountElementToBasket = StartViewBag.CountElementToBasket;
            ViewBag.CountElementToContract = StartViewBag.CountElementToContract;
            ViewBag.WrapModels = StartViewBag.WrapModels;


        }
    }
}