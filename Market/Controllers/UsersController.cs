using Market.Models;
using Market.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();

            var usersView = new List<UserView>();
            foreach (var item in users)
            {
                var userView = new UserView
                {
                    Email = item.Email,
                    Name = item.UserName,
                    UserId = item.Id,
                };
                usersView.Add(userView);
            }
            return View(usersView);
        }

        public ActionResult Roles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userId);

            if(user == null)
            {
                return HttpNotFound();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));        
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();
            
            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                rolesView.Add(roleView);
            }
            
            

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserId = userId,
                Roles = rolesView
            };
            
            return View(userView);
        }

        public ActionResult AddRole(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserId = userId,                
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var lstRole = roleManager.Roles.ToList();
            lstRole.Add(new IdentityRole { Id = "", Name = "Seleccione una opción" });
            lstRole = lstRole.OrderBy(r => r.Name).ToList();
            ViewBag.RoleId = new SelectList(lstRole, "Id", "Name");
            return View(userView);
        }

        [HttpPost]
        public ActionResult AddRole(string userId, FormCollection form)
        {
            var roleId = Request["RoleId"];

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userId);

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserId = userId,
            };
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (string.IsNullOrEmpty(roleId))
            {
                ViewBag.Error = "seleccion un rol";
                var lstRole = roleManager.Roles.ToList();
                lstRole.Add(new IdentityRole { Id = "", Name = "Seleccione una opción" });
                lstRole = lstRole.OrderBy(r => r.Id).ToList();
                ViewBag.RoleId = new SelectList(lstRole, "Id", "Name");
                return View(userView);
            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleId);
            if (!userManager.IsInRole(user.Id, role.Name))
            {
                userManager.AddToRole(userId, role.Name);
            }

            var rolesView = new List<RoleView>();
            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                rolesView.Add(roleView);
            }
            
            userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserId = userId,
                Roles = rolesView
            };

            return View("Roles", userView);
            
        }

        public ActionResult Delete(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userId);
            var role = roleManager.Roles.ToList().Find(r => r.Id == roleId);

            if (userManager.IsInRole(user.Id, role.Name))
            {
                userManager.RemoveFromRole(user.Id, role.Name);
            }

            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id ==item.RoleId);
                var roleView = new RoleView
                {
                    RoleName = role.Name,
                    RoleId = role.Id
                };
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                Roles = rolesView,
                UserId =user.Id
            };

            return View("Roles", userView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}