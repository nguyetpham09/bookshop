namespace BookShop.Data.Migrations
{
    using BookShop.Model.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookShop.Data.BookShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BookShop.Data.BookShopDbContext context)
        {
            CreateProductCategorySample(context);
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BookShopDbContext()));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BookShopDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "test",
            //    Email = "test@gmail.com",
            //    EmailConfirmed = true,
            //    Birthday = DateTime.Now,
            //    FullName = "Technology Education"
            //};

            //manager.Create(user, "123654$");

            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            //var adminUser = manager.FindByEmail("test@gmail.com");

            //manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }

        private void CreateProductCategorySample(BookShop.Data.BookShopDbContext dbContext)
        {
            if (dbContext.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory(){Name = "Điện lạnh", CreatedDate=System.DateTime.UtcNow, Alias = "dien-lanh", Status = true},
                    new ProductCategory(){Name = "Viễn thông", CreatedDate=System.DateTime.UtcNow, Alias = "vien-thong", Status = true},
                    new ProductCategory(){Name = "Đồ gia dụng", CreatedDate=System.DateTime.UtcNow, Alias = "do-gia-dung", Status = true},
                    new ProductCategory(){Name = "Mỹ phẩm", CreatedDate=System.DateTime.UtcNow, Alias = "my-pham", Status = true}
                };

                dbContext.ProductCategories.AddRange(listProductCategory);
                dbContext.SaveChanges();
            }
        }
    }
}