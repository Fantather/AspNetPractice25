using AspNetPractice25.Models;

namespace AspNetPractice25
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order? GetById(int id);
        void CreateOrder(Order order);
        bool DeleteOrder(int id);
    }
}
