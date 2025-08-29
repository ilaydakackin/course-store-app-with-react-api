using API.Entity;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public static class SeedDatabase
    {
        public static async void Initialize(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices
               .CreateScope()
               .ServiceProvider
               .GetRequiredService<UserManager<AppUser>>();

            var roleManager = app.ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<AppRole>>();

            if (!roleManager.Roles.Any())
            {
                var customer = new AppRole
                {
                    Name = "Customer"
                };
                var admin = new AppRole
                {
                    Name = "Admin"
                };

                await roleManager.CreateAsync(customer);
                await roleManager.CreateAsync(admin);

            }
            if (!userManager.Users.Any())
            {
                var admin = new AppUser
                {
                    Name = "Jane Doe",
                    UserName = "janedoe",
                    Email = "janedoe@gmail.com",
                };
                var customer = new AppUser
                {
                    Name = "John Doe",
                    UserName = "johndoe",
                    Email = "johndoe@gmail.com",
                };

                await userManager.CreateAsync(customer, "Kfdfv_l20");
                await userManager.AddToRoleAsync(customer, "Customer");

                await userManager.CreateAsync(admin, "Ghjnj_m40");
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Customer" });
            }
        } 
    }  
}