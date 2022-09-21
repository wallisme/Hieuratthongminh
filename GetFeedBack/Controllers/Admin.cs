using GetFeedBack.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GetFeedBack.Controllers
{
    public class Admin : Controller
    {
        private readonly FeedbackContext _db;

        public Admin(FeedbackContext db)
        {
            _db = db;
        }
        public IActionResult DashboardView()
        {
            var data = _db.Users
                .Join(_db.FeedBacks,
                    user => user.Id,
                    feedback => feedback.UserId,
                    (user, feedback) =>
                    new FeedbackUser
                    {
                        Id = feedback.Id,
                        Name = feedback.Name,
                        Username = user.Username,
                        Email = user.Email,
                    }
                ).ToList();
            return View(data);
        }

        public IActionResult DashboardStatistics()
        {
            List<int> userCount = new List<int>();
            List<int> fbCount = new List<int>();
            for (int i = 0; i < 12; ++i)
            {
                var temp = _db.Users.Count(p => p.CreateDate.Month == (i+1));
                var temp2 = _db.FeedBacks.Count(p => p.CreateDate.Month == (i+1));
                userCount.Add(temp);
                fbCount.Add(temp2); 
            }


            var users = _db.Users.Count();
            var fbs = _db.FeedBacks.Count();


            ViewBag.userCount = userCount.ToArray();
            ViewBag.fbCount = fbCount.ToArray();
            ViewBag.users = users;
            ViewBag.fbs = fbs;
            return View();
        }
        

        public IActionResult UserManagement()
        {
            var data = _db.Users.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]

        public IActionResult CreateUser(Users user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("UserManagement");
        }

        [HttpGet]

        public IActionResult Edit(int id)
        {
            Users user = new Users();   

            user = _db.Users.Find(id);

            _db.SaveChanges();
            return View(user);

        }

        [HttpPost]

        public IActionResult Edit (Users user)
        {
            var us = _db.Users.FirstOrDefault(x => x.Id == user.Id);
            us.Username = user.Username;
            us.Password = user.Password;
            us.Email = user.Email;
            _db.SaveChanges();
            return RedirectToAction("UserManagement");
        }

        public IActionResult Delete (int? id)
        {
            do
            {
                if (_db.FeedBacks.Count(p => p.UserId == id) == 0) break;
                FeedBacks fb = _db.FeedBacks.Where(p => p.UserId == id).FirstOrDefault();
                _db.Entry(fb).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            while (true);
            Users user = _db.Users.FirstOrDefault(p => p.Id == id);
            _db.Entry(user).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("UserManagement");
        }
    }
   
}
