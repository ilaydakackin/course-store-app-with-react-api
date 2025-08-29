namespace API.DTO;

public class CreatedAtAction{
     public class CartDTO
    {
        public int CartId { get; set; }
        public string? CustomerId {get; set;} 
        //sepetteki ürünler
        public List<CartItemDTO> CartItems {get; set;} = new();
    }
    //Sepetteki her satır CartItem'dır
     public class CartItemDTO{
        public string? Name { get; set; }
        public decimal? Price {get; set;} 
        public int ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity {get;  set;}
    }
}

