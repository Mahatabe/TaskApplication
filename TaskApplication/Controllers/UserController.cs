using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TaskApplication.Data;
using TaskApplication.Models;

namespace TaskApplication.Controllers
{
    public class UserController : Controller
    {
        DbContext userDB = new DbContext();
        // GET: User
        public ActionResult Index()
        {
            var data = userDB.GetUsers();
            return View(data);
        }
        [HttpGet]
        public ActionResult Login(String email)
        {
            var row = userDB.GetUsers().Where(model => model.emailNo == email).FirstOrDefault();
            return View(row);
        }
        [HttpPost]
        public ActionResult Login(User ul)
        {
            /* var ldata = userDB.GetUsers().Where(model => model.emailNo == ul.emailNo && model.password == ul.password).FirstOrDefault();
             if(ldata != null)
             {
                 Session["uid"] = ul.emailNo;
                 return RedirectToAction("Details");
             }   
             else
             {
                 ViewBag.Showmsg = "Invalid";
                 ModelState.Clear();
             }*/
            if (ul.emailNo == "admin@admin.com" && ul.password == "@123@")
            {
                Session["uid"] = ul.emailNo;
                
                return RedirectToAction("Index");
            }
            else
            {
                var ldata = userDB.GetUsers().Where(model => model.emailNo == ul.emailNo && model.password == ul.password).FirstOrDefault();
                if (ldata != null)
                {
                    Session["uid"] = ul.emailNo;
                    return RedirectToAction("Details");
                }
                else
                {
                    ViewBag.Showmsg = "Invalid";
                    ModelState.Clear();
                }
            }
            return View();
            
        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user, HttpPostedFileBase imageFile, HttpPostedFileBase cvFile)
        {
            if (userDB.InsertUser(user, imageFile, cvFile))
            {
                TempData["InsertMsg"] = "User create succesful";
                Session["uid"] = user.emailNo;
                return RedirectToAction("Details");
            }
            else
            {
                TempData["InsertErrorMsg"] = "User create unsuccesful";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var data = userDB.GetUsers().Where(x => x.userId == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(User user, HttpPostedFileBase imageFile, HttpPostedFileBase cvFile)
        {
                /*if (userDB.UpdateUser(user, imageFile, cvFile))
                {
                    TempData["UpdateMsg"] = "User updated successfully";
                    Session["uid"] = user.emailNo;
                    return RedirectToAction("Details");
                }
                else
                {
                    TempData["UpdateErrorMsg"] = "Failed to update user";
                }*/
            userDB.UpdateUser(user, imageFile, cvFile);
            return View();
        }


        public ActionResult Details(User user)
        {
            var row = userDB.GetUsers().Where(model => model.emailNo == Session["uid"].ToString()).FirstOrDefault();
            return View(row);

            
        }
    }
}