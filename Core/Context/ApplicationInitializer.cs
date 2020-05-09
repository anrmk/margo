using System;
using System.Collections.Generic;
using System.Linq;

using Core.Data.Entities;

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

            Section();

            string rootPath = System.IO.Directory.GetCurrentDirectory();
        }

        private void RoleManager() {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!roleManager.RoleExistsAsync("Administrator").Result) {
                IdentityResult roleResult = roleManager.CreateAsync(new IdentityRole() {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }).Result;
            }

            if(!roleManager.RoleExistsAsync("User").Result) {
                IdentityResult roleResult = roleManager.CreateAsync(new IdentityRole() {
                    Name = "User",
                    NormalizedName = "USER"
                }).Result;
            }
        }

        private void ApplicationUser() {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUserEntity>>();

            if(userManager.FindByEmailAsync("test@test.com").Result == null) {
                var user = new ApplicationUserEntity() {
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
        }

        private void Section() {
            var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
            var sections = _context.Sections.ToList();

            if(sections.Count == 0) {
                var newSections = new List<SectionEntity>() {
                    new SectionEntity() { IsDefault = true, Sort = 1, Name = "Addresses", Description = "" },
                    new SectionEntity() { IsDefault = true, Sort = 2, Name = "Phones", Description = "" },
                    new SectionEntity() { IsDefault = true, Sort = 3, Name = "Emails", Description = "An email list is a collection of email addresses of business."},
                    new SectionEntity() { IsDefault = true, Sort = 4, Name = "Social Media", Description = "" }
                };
                _context.Sections.AddRange(newSections);
                _context.SaveChanges();
            }
        }
    }
}
