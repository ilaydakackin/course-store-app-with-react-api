using API.Data;
using API.Entity;
using API.DTO;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API.DTO.CreatedAtAction;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;
        public CartController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<CartDTO>> GetCart()
        {
            var cart = await GetOrCreate(GetCustomerId());

            return CartToDTO(cart);
        }
         [HttpPost]
        public async Task<IActionResult> AddItemToCart(int productId, int quantity)
        {
           var cart = await GetOrCreate(GetCustomerId());
           var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == productId);
                if(product == null)
                {
                    return NotFound();
                }
            cart.AddItem(product, quantity);
            var result = await _context.SaveChangesAsync() > 0;

            if(result)
                return CreatedAtAction(nameof(GetCart), CartToDTO(cart));
            
            return BadRequest(new ProblemDetails{Title = "The product can not be added to cart"});

        }

        [HttpDelete]
          public async Task<IActionResult> DeleteItemFromCart(int productId, int quantity)
        {
           var cart = await GetOrCreate(GetCustomerId());
           var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == productId);
                if(product == null)
                {
                    return NotFound();
                }
            cart.DeleteItem(productId, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            
             if(result)
                return CreatedAtAction(nameof(GetCart), CartToDTO(cart));
            if(result)

            return Ok();
            
            return BadRequest( new ProblemDetails { Title = "Problem removing item from the cart"});
        }

        private string GetCustomerId()
        {
            return User.Identity?.Name ?? Request.Cookies["customerId"]!;
          
        }
        private async Task<Cart> GetOrCreate(string custId)
        {
            var cart = await _context.Carts
               .Include(i => i.CartItems)
               .ThenInclude(i => i.Product)
               .Where(i => i.CustomerId == custId)
               .FirstOrDefaultAsync();

            if (cart == null)
            {
                var customerId = User.Identity?.Name;
                if (string.IsNullOrEmpty(customerId))
                {

                    customerId = Guid.NewGuid().ToString();
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMonths(1),
                        IsEssential = true,
                    };
                Response.Cookies.Append("customerId", customerId, cookieOptions); 
                }
               
                cart = new Cart { CustomerId = customerId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        private CartDTO CartToDTO(Cart cart)
        {
            return new CartDTO
            {
                CartId = cart.CartId,
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems.Select(item => new CartItemDTO{
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.Product.ImageUrl
                }).ToList()
            };
        }
        
    }
}
