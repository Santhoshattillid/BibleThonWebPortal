using System.Linq;
using AlbaDL;

namespace TestConsole
{
    internal class Program
    {
        private static void Main()
        {
            var twoEntities = new TWOEntities();
            var ord = new OrderDetail
            {
                CustomerName = "test",
                FormData = "Test",
                Operator = "Test",
                Status = "Test",
                Id = twoEntities.OrderDetails.Max(id => id.Id) + 1
            };

            twoEntities.OrderDetails.AddObject(ord);
            twoEntities.SaveChanges();
        }
    }
}