using AspNetPractice25.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetPractice25.Controllers
{
    public class OrdersController(IOrderRepository orderRepository) : Controller
    {
        public IActionResult Index()
        {
            return View(orderRepository.GetAllOrders());
        }

        public IActionResult Details(int id)
        { 
            Order? order = orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult Add(Order order)
        {
            if(!ModelState.IsValid)
            {
                return View(order);
            }
            orderRepository.CreateOrder(order);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            bool result = orderRepository.DeleteOrder(id);
            if(result)
            {
                return RedirectToAction("Index");
            }

            return Content("Не удалось удалить");
        }
    }
}
