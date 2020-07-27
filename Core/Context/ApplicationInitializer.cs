using System;
using System.Collections.Generic;
using System.Linq;

using Core.Data.Entities;
using Core.Data.Enums;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Context {
    public class ApplicationInitializer {
        private readonly IServiceProvider _serviceProvider;

        public static ApplicationInitializer Initialize(IServiceProvider serviceProvider) {
            return new ApplicationInitializer(serviceProvider);
        }

        public ApplicationInitializer(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;

            RoleManager();
            ApplicationUser();

            Initialize();

            string rootPath = System.IO.Directory.GetCurrentDirectory();
        }

        private void RoleManager() {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!roleManager.RoleExistsAsync("Administrator").Result) {
                roleManager.CreateAsync(new IdentityRole() {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
            }

            if(!roleManager.RoleExistsAsync("User").Result) {
                roleManager.CreateAsync(new IdentityRole() {
                    Name = "User",
                    NormalizedName = "USER"
                });
            }
        }

        private void ApplicationUser() {
            var userManager = _serviceProvider.GetRequiredService<UserManager<AppNetUserEntity>>();

            if(userManager.FindByEmailAsync("test@test.com").Result == null) {
                var user = new AppNetUserEntity() {
                    UserName = "test@test.com",
                    NormalizedUserName = "ADMINISTRATOR",
                    Email = "test@test.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "123qweAS1!").Result;
                if(result.Succeeded) {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }

            if(userManager.FindByEmailAsync("user@user.com").Result == null) {
                var user = new AppNetUserEntity() {
                    UserName = "user@user.com",
                    NormalizedUserName = "USER",
                    Email = "user@user.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "123qweAS1!").Result;
                if(result.Succeeded) {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }

        private void Initialize() {
            var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
            var sections = _context.Categories.ToList();

            if(sections.Count == 0) {
                var newSections = new List<CategoryEntity>() {
                     new CategoryEntity() { Name = "Email", Fields = new List<CategoryFieldEntity>(){
                        new CategoryFieldEntity() {Name="Login", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name="Password", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name="Account", Type = FieldEnum.TEXT },
                        new CategoryFieldEntity() {Name="Note", Type = FieldEnum.TEXT }
                    }},

                    new CategoryEntity() { Name = "Phone", Fields = new List<CategoryFieldEntity>() {
                        new CategoryFieldEntity() {Name="Phone Number", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name="Provider", Type = FieldEnum.TEXT },
                        new CategoryFieldEntity() {Name="Secure Question", Type = FieldEnum.TEXT },
                        new CategoryFieldEntity() {Name="Device", Type = FieldEnum.TEXT }
                    }},

                    new CategoryEntity() {Name = "VoIP", Fields = new List<CategoryFieldEntity>() {
                        new CategoryFieldEntity() {Name = "Phone Number", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Server", Type = FieldEnum.TEXT },
                        new CategoryFieldEntity() {Name = "Login", Type = FieldEnum.TEXT },
                        new CategoryFieldEntity() {Name = "Password", Type = FieldEnum.PASSWORD },
                    }},

                    new CategoryEntity() { Name = "Fax", Fields = new List<CategoryFieldEntity>() {
                        new CategoryFieldEntity() {Name = "Fax Number", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name = "User Name", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Password", Type = FieldEnum.PASSWORD, IsRequired = true}
                    }},

                    new CategoryEntity() { Name = "Social Media", Fields = new List<CategoryFieldEntity>() {
                        new CategoryFieldEntity() {Name = "Email", Type = FieldEnum.EMAIL, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Password", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Login/User Name", Type = FieldEnum.TEXT},
                        new CategoryFieldEntity() {Name = "Comment", Type = FieldEnum.TEXT},
                        new CategoryFieldEntity() {Name = "Link", Type = FieldEnum.LINK},
                    }},

                     new CategoryEntity() { Name = "Websites", Fields = new List<CategoryFieldEntity>() {
                        new CategoryFieldEntity() {Name = "Username", Type = FieldEnum.TEXT, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Password", Type = FieldEnum.PASSWORD, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Link", Type = FieldEnum.LINK},
                     }},

                     new CategoryEntity() { Name = "Hosting & Domain", Fields = new List<CategoryFieldEntity>() {
                        new CategoryFieldEntity() {Name = "Email", Type = FieldEnum.EMAIL, IsRequired = true },
                        new CategoryFieldEntity() {Name = "Username", Type = FieldEnum.TEXT},
                        new CategoryFieldEntity() {Name = "Password", Type = FieldEnum.PASSWORD, IsRequired = true },
                     }},
                };
                _context.Categories.AddRange(newSections);
                _context.SaveChanges();
            }

            var vendors = _context.Vendors.ToList();
            if(vendors.Count == 0) {
                var newVendors = new List<VendorEntity>() {
                    new VendorEntity() { No="045554823",  Name = "T Roberts Fabrics INC" },
                    new VendorEntity() { No="622824209",  Name = "Water Purification Consultants", Fields = new List<VendorFieldEntity>() {
                        new VendorFieldEntity() {IsRequired = true, Name = "Field 1", Type = FieldEnum.TEXT },
                        new VendorFieldEntity() {IsRequired = true, Name = "Field 2", Type = FieldEnum.NUMBER },
                        new VendorFieldEntity() {IsRequired = true, Name = "Field 3", Type = FieldEnum.DATE },
                        new VendorFieldEntity() {IsRequired = false, Name = "Field 1", Type = FieldEnum.TEXT },
                        }
                    },
                    new VendorEntity() { No="Not Specified",  Name = "Springs Enterprises LLC" },

                };
                _context.Vendors.AddRange(newVendors);
                _context.SaveChanges();
            }
        }
    }
}
