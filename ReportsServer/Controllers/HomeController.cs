using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using ReportsServer.Entities;
using ReportsServer.Models;

namespace ReportsServer.Controllers
{
    public class HomeController : Controller
    {
        private JsonDatabase<User> _users;
        private JsonDatabase<Session> _sessions;
        private JsonDatabase<Task> _tasks;
        private JsonDatabase<Report> _reports;
        private HttpServerUtility _server = System.Web.HttpContext.Current.Server;
        
        public HomeController()
        {
            _users = new JsonDatabase<User>(_server.MapPath("~/App_Data/Users/"));
            _sessions = new JsonDatabase<Session>(_server.MapPath("~/App_Data/Sessions/"));
            _tasks = new JsonDatabase<Task>(_server.MapPath("~/App_Data/Tasks/"));
            _reports = new JsonDatabase<Report>(_server.MapPath("~/App_Data/Reports/"));
        }

        public ActionResult Tasks()
        {
            RestoreSession();
            if (!ViewBag.LoginSuccessfull)
                return View();
            ViewBag.Tasks = _tasks.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult GetUser(string uid)
        {
            RestoreSession();
            if (!ViewBag.LoginSuccessfull)
                return View();
            ViewBag.User = _users.Get(uid);
            ViewBag.Subordinates = _users.GetAll(u => u.Boss == uid).Select(u => u.Uid);
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Tasks(string name)
        {
            Session session = RestoreSession();

            if (!ViewBag.LoginSuccessfull)
                return View();

            _tasks.Create(new Task()
            {
                Uid = _tasks.GetAll().Count.ToString(), 
                Title = name, 
                State = TaskState.Open,
                Creator = session.Username, 
                CreationDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            });
            ViewBag.Tasks = _tasks.GetAll();
            
             return View();
        }

        [HttpGet]
        public ActionResult Edit(string uid)
        {
            RestoreSession();
            if (!ViewBag.LoginSuccessfull)
                return View();

            ViewBag.Task = _tasks.Get(uid);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string uid, string title, string status)
        {
            RestoreSession();
            if (!ViewBag.LoginSuccessfull)
                return View();
            Task task = _tasks.Get(uid);
            task.Title = title;
            task.State = (TaskState) int.Parse(status);
            task.LastChangedDate = DateTime.Now;
            _tasks.Update(task);
            ViewBag.Task = _tasks.Get(uid);
            return View();
        }

        public ActionResult Reports()
        {
            Session session = RestoreSession();

            if (!ViewBag.LoginSuccessfull)
                return View();
            
            ViewBag.Reports = _reports.GetAll();

            return View();
        }

        [HttpPost]
        public ActionResult Reports(string action)
        {
            Session session = RestoreSession();

            if (!ViewBag.LoginSuccessfull)
                return View();
            
            if(action == "create")
                _reports.Create(new Report()
                {
                    Uid = _reports.GetAll().Count.ToString(),
                    Creator = session.Username,
                    CreationDate = DateTime.Now,
                    State = false,
                    Tasks = new List<string>(),
                    Reports = new List<string>()
                });
            ViewBag.Reports = _reports.GetAll();

            return View();
        }
        
        [HttpGet]
        public ActionResult EditReport(string uid)
        {
            Session session = RestoreSession();
            
            if (!ViewBag.LoginSuccessfull)
                return View();
            
            ViewBag.CurrentUser = session.Username;
            Report report = _reports.Get(uid);
            ViewBag.Report = report;
            MakeReportView(report);
            return View();
        }

        [HttpPost]
        public ActionResult EditReport(string uid, string[] reports, string[] tasks, string action)
        {
            Session session = RestoreSession();
            if (!ViewBag.LoginSuccessfull)
                return View();
            Report report = _reports.Get(uid);
            if(reports != null)
                report.Reports = reports.Select(r => r.Split(':')[0]).ToList();
            if(tasks != null)
                report.Tasks = tasks.Select(r => r.Split(':')[0]).ToList();
            if (action == "Submit")
                report.State = true;
            _reports.Update(report);

            ViewBag.Report = report;

            ViewBag.CurrentUser = session.Username;
            
            MakeReportView(report);
            
            return View();
        }

        public ActionResult Index()
        {
            RestoreSession();
            return View();
        }

        public ActionResult Register()
        {
            RestoreSession();
            ViewBag.Usernames = _users.GetAll().Select(s => s.Uid).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(string login, string boss, string password)
        {
            if (_users.Get(login) != null)
            {
                ViewBag.Message = "Choose another nickname";
                return Register();
            }
            var user = new User(login, password, boss);
            _users.Create(user);
            StartSession(user.Uid);
            ViewBag.Message = "Welcome, " + login;
            return View();
        }

        public ActionResult Login()
        {
            RestoreSession();
            return View();
        }
        
        public ActionResult Logout()
        {
            Session restoredSession = RestoreSession();
            if (restoredSession != null)
            {
                _sessions.Remove(restoredSession);
            }
            ViewBag.LoginSuccessfull = false;

            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            if (RestoreSession() != null)
                return View();

            User user = _users.Get(login);
            if (user == null)
            {
                ViewBag.Message = "User not registered";
                return View();
            }
            if (user.Password != password)
            {
                ViewBag.Message = "Wrong password";
                return View();
            }

            StartSession(user.Uid);
            ViewBag.Message = "Welcome back, " + login;

            return View();
        }

        private Session RestoreSession()
        {
            ViewBag.LoginSuccessfull = false;
            HttpCookie httpCookie = Request.Cookies.Get("session");
            if (httpCookie != null)
            {
                Session session = _sessions.Get(httpCookie.Value);
                if (session != null)
                {
                    ViewBag.Message = "Welcome back, " + session.Username;
                    ViewBag.LoginSuccessfull = true;
                    return session;
                }
            }

            return null;
        }

        void StartSession(string login)
        {
            var session = new Session(login);
            _sessions.Create(new Session(login));
            Response.Cookies.Add(new HttpCookie("session", session.Uid));
            ViewBag.LoginSuccessfull = true;
        }

        private void MakeReportView(Report report)
        {
            ViewBag.AddedReports = _reports.GetAll(r => report.Reports.Contains(r.Uid))
                .Select(r => r.Creator + " - " + r.CreationDate);
            ViewBag.Reports = _reports.GetAll(r => _users.Get(r.Creator).Boss == report.Creator && r.State)
                .Select(r => r.Uid + ": " +  r.Creator + " - " + r.CreationDate);

            ViewBag.AddedTasks = _tasks.GetAll(t => report.Tasks.Contains(t.Uid))
                .Select(t => t.Title + " - " + t.CreationDate);
            ViewBag.Tasks = _tasks.GetAll(t => t.Creator == report.Creator)
                .Select(t => t.Uid + ": " +  t.Title + " - " + t.State);
        }
    }
    
}