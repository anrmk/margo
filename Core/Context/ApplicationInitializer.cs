using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            Thread.Sleep(2000);
            ApplicationUser();
            Thread.Sleep(2000);
            Initialize();

            //string rootPath = System.IO.Directory.GetCurrentDirectory();
        }

        private void RoleManager() {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!roleManager.RoleExistsAsync("Administrator").Result) {
                roleManager.CreateAsync(new IdentityRole() {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }).Wait(1000);
            }

            if(!roleManager.RoleExistsAsync("User").Result) {
                roleManager.CreateAsync(new IdentityRole() {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                }).Wait(1000);
            }
        }

        private void ApplicationUser() {
            var userManager = _serviceProvider.GetRequiredService<UserManager<AspNetUserEntity>>();

            if(userManager.FindByEmailAsync("test@test.com").Result == null) {
                var user = new AspNetUserEntity() {
                    UserName = "test@test.com",
                    NormalizedUserName = "ADMINISTRATOR",
                    Email = "test@test.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "123qweAS1!").Result;
                if(result.Succeeded) {
                    userManager.AddToRoleAsync(user, "Administrator").Wait(1000);
                }
            }

            if(userManager.FindByEmailAsync("user@user.com").Result == null) {
                var user = new AspNetUserEntity() {
                    UserName = "user@user.com",
                    NormalizedUserName = "TEST USER",
                    Email = "user@user.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "!q2w3e4r").Result;
                if(result.Succeeded) {
                    userManager.AddToRoleAsync(user, "Manager").Wait(1000);
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

                     new CategoryEntity() { Name = "Phone" },

                    new CategoryEntity() { Name = "Mobile", Fields = new List<CategoryFieldEntity>() {
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
        }
    }
}
