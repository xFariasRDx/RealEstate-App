﻿using Microsoft.AspNetCore.Identity;
using RSApp.Core.Services.Enums;
using RSApp.Infrastructure.Identity.Entities;

namespace RSApp.Infrastructure.Identity.Seeds;
public static class DefaultRoles {
  public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
    await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Dev.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Agent.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
  }
}

