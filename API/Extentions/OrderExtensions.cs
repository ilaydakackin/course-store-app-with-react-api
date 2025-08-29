using API.DTO;
using API.Entity;


namespace API.Extentions
{
    public static class OrderExtensions
    {
        public static IQueryable<OrderDTO> OrderToDTO(this IQueryable<Order> query)
        {
            return query.Select(i => new OrderDTO
            {
                Id = i.Id,
                OrderDate = i.OrderDate,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Phone = i.Phone,
                City = i.City,
                AdresLine = i.AdresLine,
                CustomerId = i.CustomerId,
                OrderStatus = i.OrderStatus,
                SubTotal = i.SubTotal,
                DeliveryFree = i.DeliveryFree,
                OrderItems = i.OrderItems.Select(i => new OrderItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductImageUrl = i.ProductImageUrl,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        public static OrderDTO OrderToDTO(this Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Phone = order.Phone,
                City = order.City,
                AdresLine = order.AdresLine,
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus,
                SubTotal = order.SubTotal,
                DeliveryFree = order.DeliveryFree,
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemDTO
                {
                    Id = orderItem.Id,
                    ProductId = orderItem.ProductId,
                    ProductName = orderItem.ProductName,
                    ProductImageUrl = orderItem.ProductImageUrl,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity
                }).ToList()
            };
        }
    }
}