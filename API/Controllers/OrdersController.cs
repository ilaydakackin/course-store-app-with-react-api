
using API.Data;
using API.DTO;
using API.Entity;
using API.Extentions;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   [Authorize]
   [ApiController]
   [Route("/api/[controller]")]
   public class OrdersController : ControllerBase
   {
      private readonly DataContext _context;
      private IConfiguration _config;
      public OrdersController(DataContext context, IConfiguration config)
      {
         _context = context;
         _config = config;
      }

      [HttpGet]
      public async Task<ActionResult<List<OrderDTO>>> GetOrders()
      {
         Console.WriteLine($"User: {User.Identity!.Name}");
         return await _context.Orders
            .Include(o => o.OrderItems)
            .OrderToDTO()
            .Where(o => o.CustomerId == User.Identity!.Name)
            .ToListAsync();

      }

      [HttpGet("{id}")]
      public async Task<ActionResult<OrderDTO?>> GetOrder(int id)
      {
         return await _context.Orders
          .Include(o => o.OrderItems)
          .OrderToDTO()
                   .Where(o => o.CustomerId == User.Identity!.Name && o.Id == id)

          .FirstOrDefaultAsync();
      }

      [HttpPost]
      public async Task<ActionResult<Order>> CreateOrder(CreateOrderDTO orderDTO)
      {
         var cart = await _context.Carts
             .Include(c => c.CartItems)
             .ThenInclude(ci => ci.Product)
             .Where(i => i.CustomerId == User.Identity!.Name)
             .FirstOrDefaultAsync();

         if (cart == null)
         {
            return BadRequest(new ProblemDetails { Title = "Sepet bulunamadı" });
         }
         var items = new List<Entity.OrderItem>();

         foreach (var item in cart.CartItems)
         {
            var product = await _context.Products.FindAsync(item.ProductId);

            var orderItem = new Entity.OrderItem
            {
               ProductId = product!.Id.ToString(),
               ProductName = product!.Name,
               ProductImageUrl = product.ImageUrl,
               Price = product.Price ?? 0,
               Quantity = item.Quantity
            };
            items.Add(orderItem);
            product.Stock -= item.Quantity;

         }

         var SubTotal = items.Sum(i => i.Price * i.Quantity);
         var deliveryFee = 0;

         var order = new Order
         {
            OrderItems = items,
            CustomerId = User.Identity!.Name,
            FirstName = orderDTO.FirstName,
            LastName = orderDTO.LastName,
            Phone = orderDTO.Phone,
            City = orderDTO.City,
            AdresLine = orderDTO.AdresLine,
            SubTotal = SubTotal,
            DeliveryFree = deliveryFee,
         };

         //payment
         var paymentResult = await ProcessPayment(orderDTO,  cart);

         if (paymentResult.Status == "failure")
         {
            return BadRequest(new ProblemDetails { Title = paymentResult.ErrorMessage });
         }

         order.ConversationId = paymentResult.ConversationId;
         order.BasketId = paymentResult.BasketId;
         _context.Orders.Add(order);
         _context.Carts.Remove(cart);

         var result = await _context.SaveChangesAsync() > 0;
         if (result)
         {
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order.OrderToDTO());
         }
         else
         {
            return BadRequest(new ProblemDetails { Title = "Sipariş oluşturulamadı" });
         }
      }

      private async Task<Payment> ProcessPayment(CreateOrderDTO model, Cart cart)
      {
         Options options = new Options();
         options.ApiKey = _config["PaymentApi:ApiKey"];
         options.SecretKey = _config["PaymentApi:SecretKey"];
         options.BaseUrl = "https://sandbox-api.iyzipay.com";

         CreatePaymentRequest request = new CreatePaymentRequest();
         request.Locale = Locale.TR.ToString();
         request.ConversationId = Guid.NewGuid().ToString();
         request.Price = cart.CalculateTotal().ToString();
         request.PaidPrice = cart.CalculateTotal().ToString();
         request.Currency = Currency.TRY.ToString();
         request.Installment = 1;
         request.BasketId = cart.CartId.ToString();
         request.PaymentChannel = PaymentChannel.WEB.ToString();
         request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

         PaymentCard paymentCard = new PaymentCard();
         paymentCard.CardHolderName = model.CardName;
         paymentCard.CardNumber = model.CardNumber;
         paymentCard.ExpireMonth = model.CardExpireMonth;
         paymentCard.ExpireYear = model.CardExpireYear;
         paymentCard.Cvc = model.CardCvc;
         paymentCard.RegisterCard = 0;
         request.PaymentCard = paymentCard;

         Buyer buyer = new Buyer();
         buyer.Id = "BY789";
         buyer.Name = model.FirstName;
         buyer.Surname = model.LastName;
         buyer.GsmNumber = model.Phone;
         buyer.Email = "email@email.com";
         buyer.IdentityNumber = "74300864791";
         buyer.LastLoginDate = "2015-10-05 12:43:35";
         buyer.RegistrationDate = "2013-04-21 15:12:09";
         buyer.RegistrationAddress = model.AdresLine;
         buyer.City = model.City;
         buyer.Country = "Türkiye";
         buyer.ZipCode = "34732";
         request.Buyer = buyer;

         Address shippingAddress = new Address();
         shippingAddress.ContactName = model.FirstName + " " + model.LastName;
         shippingAddress.City = model.City;
         shippingAddress.Country = "Türkiye";
         shippingAddress.Description = model.AdresLine;
         shippingAddress.ZipCode = "34732";

         request.ShippingAddress = shippingAddress;
         request.BillingAddress = shippingAddress;

         List<BasketItem> basketItems = new List<BasketItem>();
         foreach (var item in cart.CartItems)
         {
            BasketItem basketItem = new BasketItem();
            basketItem.Id = item.ProductId.ToString();
            basketItem.Name = item.Product.Name;
            basketItem.Category1 = "saat";
            basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            basketItem.Price = ((double)item.Product.Price * item.Quantity).ToString();
            basketItems.Add(basketItem);
         }
         request.BasketItems = basketItems;

         return await Payment.Create(request, options);

      }


   }

}
