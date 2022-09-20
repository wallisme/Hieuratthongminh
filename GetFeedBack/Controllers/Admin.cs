using GetFeedBack.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();
            List<int> userCount = new List<int>();
            List<int> fbCount = new List<int>();
            for (int i = 1; i <= 6; ++i)
            {
                var temp = _db.Users.Count(p => p.CreateDate.Month == i);
                var temp2 = _db.FeedBacks.Count(p => p.CreateDate.Month == i);
                userCount.Add(temp);
                fbCount.Add(temp2);
            }


            dataPoints1.Add(new DataPoint("Jan", userCount[0]));
            dataPoints1.Add(new DataPoint("Feb", userCount[1]));
            dataPoints1.Add(new DataPoint("Mar", userCount[2]));
            dataPoints1.Add(new DataPoint("Apr", userCount[3]));
            dataPoints1.Add(new DataPoint("May", userCount[4]));
            dataPoints1.Add(new DataPoint("Jun", userCount[5]));

            dataPoints2.Add(new DataPoint("Jan", fbCount[0]));
            dataPoints2.Add(new DataPoint("Feb", fbCount[1]));
            dataPoints2.Add(new DataPoint("Mar", fbCount[2]));
            dataPoints2.Add(new DataPoint("Apr", fbCount[3]));
            dataPoints2.Add(new DataPoint("May", fbCount[4]));
            dataPoints2.Add(new DataPoint("Jun", fbCount[5]));

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

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
            var update = (from v in _db.Users where v.Id == user.Id select v).FirstOrDefault();
            update.Equals(user);
            return RedirectToAction("UserManagement");
        }

        public IActionResult Delete (int? id)
        {
            Users user = _db.Users.Find(id);

            var listItem = _db.FeedBacks.First(x => x.UserId == id);
            if (listItem != null)
            {
                _db.FeedBacks.Remove(listItem);
            }
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("UserManagement");
        }
    }
   
}
