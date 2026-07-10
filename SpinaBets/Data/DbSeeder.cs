using Microsoft.AspNetCore.Identity;
using SpinaBets.Models;

namespace SpinaBets.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAsync(
            IServiceProvider services)
        {
            var roleManager =
                services.GetRequiredService
                <RoleManager<IdentityRole>>();

            var userManager= services.GetRequiredService
                <UserManager < ApplicationUser >> ();

            string[] roles =
            {
                "Admin",
                "Customer"
            };

            foreach (var role in roles)
            {
                if (!await roleManager
                    .RoleExistsAsync(role))
                {
                    await roleManager
                        .CreateAsync(
                            new IdentityRole(role));
                }
            }
            string adminEmail = "admin@Spinabets.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // create the admin user
                var user = new ApplicationUser()
                {
                    FirstName = "Admin",
                    Surname = "Admin",
                    UserName = adminEmail, // UserName will be used to authenticate the user
                    Email = adminEmail,
                    CreatedDate = DateTime.Now,
                };

                string initialPassword = "Admin123!";


                var result = await userManager.CreateAsync(user, initialPassword);
                if (result.Succeeded)
                {
                    // set the user role
                    await userManager.AddToRoleAsync(user, "Admin");
                    Console.WriteLine("Admin user created successfully! Please update the initial password!");
                    Console.WriteLine("Email: " + user.Email);
                    Console.WriteLine("Initial password: " + initialPassword);
                }
            }
        }
    }
}
