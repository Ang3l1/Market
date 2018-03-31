using Market.Models;
using Market.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class OrdersController : Controller
    {
        MarketContext db = new MarketContext();
        // GET: Orders
        public ActionResult NewOrder()
        {
            var orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();

            Session["orderView"] = orderView;

            var lstCustomer = db.Customers.ToList();
            lstCustomer.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
            ViewBag.CustomerId = new SelectList(lstCustomer.OrderBy(c => c.FirstName), "CustomerId", "FullName");

            return View(orderView);
        }


        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {
            orderView = Session["orderView"] as OrderView;

            var customerId = int.Parse(Request["CustomerId"]);
            if (customerId == 0)
            {
                var lstCustomers = db.Customers.ToList();
                lstCustomers.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
                ViewBag.CustomerId = new SelectList(lstCustomers.OrderBy(c => c.FirstName), "CustomerId", "FullName");
                ViewBag.Error = "seleccione un cliente";

                return View(orderView);
            }

            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                var lstCustomers = db.Customers.ToList();
                lstCustomers.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
                ViewBag.CustomerId = new SelectList(lstCustomers.OrderBy(c => c.FirstName), "CustomerId", "FullName");
                ViewBag.Error = "cliente no existe";

                return View(orderView);
            }

            if (orderView.Products.Count == 0)
            {
                var lstCustomers = db.Customers.ToList();
                lstCustomers.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
                ViewBag.CustomerId = new SelectList(lstCustomers.OrderBy(c => c.FirstName), "CustomerId", "FullName");
                ViewBag.Error = "ningún producto agregado en detalle";

                return View(orderView);
            }

            var orderId = 0;

            using (var transaction =db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        CustomerId = customerId,
                        OrderDate = DateTime.Now,
                        OrderStatus = OrderStatus.Created,
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderId = db.Orders.ToList().Select(o => o.OrderId).Max();
                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Id = item.Id,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            OrderId = orderId
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    ViewBag.Error = "Error" + ex.Message;
                    var lstCustomers = db.Customers.ToList();
                    lstCustomers.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
                    ViewBag.CustomerId = new SelectList(lstCustomers.OrderBy(c => c.FirstName), "CustomerId", "FullName");
                    return View(orderView);
                }
            }

            
            
            ViewBag.Message = string.Format("Orden {0} registrada!", orderId);

            var lstCustomer = db.Customers.ToList();
            lstCustomer.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
            ViewBag.CustomerId = new SelectList(lstCustomer.OrderBy(c => c.FirstName), "CustomerId", "FullName");

            orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();

            Session["orderView"] = orderView;

            //RedirectToAction("NewOrder");
            return View(orderView);
        }

        public ActionResult AddProduct()
        {
            var lstproduct = db.Products.ToList();
            lstproduct.Add(new ProductOrder { Id = 0, Description = "[Seleccione una opción]" });
            ViewBag.ProductId = new SelectList(lstproduct.OrderBy(c => c.Description), "Id", "Description");
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            var orderView = Session["orderView"] as OrderView;

            var productId = int.Parse(Request["ProductId"]);
            if (productId == 0)
            {
                var lstproduct = db.Products.ToList();
                lstproduct.Add(new ProductOrder { Id = 0, Description = "[Seleccione una opción]" });
                ViewBag.ProductId = new SelectList(lstproduct.OrderBy(c => c.Description), "Id", "Description");
                ViewBag.Error = "debe seleccionar producto";
                return View(productOrder);
            }

            var product = db.Products.Find(productId);
            if (product == null)
            {
                var lstproduct = db.Products.ToList();
                lstproduct.Add(new ProductOrder { Id = 0, Description = "[Seleccione una opción]" });
                ViewBag.ProductId = new SelectList(lstproduct.OrderBy(c => c.Description), "Id", "Description");
                ViewBag.Error = "producto no existe";
                return View(productOrder);
            }

            productOrder = orderView.Products.Find(po => po.Id == productId);
            if (productOrder == null)
            {
                productOrder = new ProductOrder
                {
                    Id = product.Id,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = float.Parse(Request["Quantity"]),
                };
                orderView.Products.Add(productOrder);
            }
            else
            {
                productOrder.Quantity += float.Parse(Request["Quantity"]);
            }
            

            var lstCustomer = db.Customers.ToList();
            lstCustomer.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione una opción]" });
            ViewBag.CustomerId = new SelectList(lstCustomer.OrderBy(c => c.FirstName), "CustomerId", "FullName");
            return View("NewOrder", orderView);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}