using Microsoft.AspNetCore.Mvc;

using Stripe.Checkout;
using StripeDemo.Models;

namespace StripeDemo.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product="Tommy Hilfigr",
                    Rate=1500,
                    Quantity=2,
                    ImagePath="img/Image1.png"
                },
                 new ProductEntity
                {
                    Product="TimeWear",
                    Rate=2500,
                    Quantity=1,
                    ImagePath="img/Image2.png"
                }

            };
            return View(productList);
        }
        public IActionResult orderConfirmation() 
        {
            var service =new  SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString(); 
                return View("Sucess");
            }
            return View("Login");
        }
        public IActionResult Sucess()
        {
                return View();
        }
         public IActionResult Login()
        {
            return View();
        }
        
            public IActionResult CheckOut()
        {
            List<ProductEntity> productList = new List<ProductEntity>
            {

                new ProductEntity
                {
                    Product = "Tommy Hilfigr",
                    Rate = 1500,
                    Quantity = 2,
                    ImagePath = "img/Image1.png"
                },
                new ProductEntity
                {
                    Product = "TimeWear",
                    Rate = 2500,
                    Quantity = 1,
                    ImagePath = "img/Image2.png"
                }
            };
            var domain = "https://localhost:44354/";
            var options = new SessionCreateOptions
            {
                SuccessUrl=domain+$"Checkout/orderConfirmation",
                CancelUrl = domain + $"Checkout/Login",
                LineItems=new List<SessionLineItemOptions>(),
                Mode="payment"

            };
            foreach(var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quantity),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionListItem);
            }
            var service= new SessionService();  
            Session session=service.Create(options);
            TempData["Session"] = session.Id;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


    }
}
