using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chatter.Models;
using Newtonsoft.Json;

namespace Chatter.Controllers
{
    public class ChatsController : Controller
    {
        private ChatterEntities db = new ChatterEntities();

        // GET: Chats
        [Authorize][HttpsRequire]
        public ActionResult Index()
        {
            var chats = db.Chats.Include(c => c.AspNetUser);
            return View(chats.ToList());



        }
        
        public JsonResult TestJson()
        {
            //string jsonTest = "{ \"firstName\": \"Bob\", \"lastName\": \"Sauce\", \"children\": [{\"firstName\": \"Barbie\", \"age\": 19 },{\"firstName\": \"Ron\", \"age\": null }] }";

            //var chats = from Chats in db.Chats
            //            orderby
            //            Chats.Timestamp descending
            //            select new
            //            {
            //                Chats.Message,
            //                Chats.AspNetUser.UserName
            //            };

            //var output = JsonConvert.SerializeObject(chats.ToList());

            var chats = from Chats in db.Chats
                        orderby
                        Chats.Timestamp descending
                        //Selects Message and UserName from the Chats 
                        select new
                        {
                            Chats.Message,
                            Chats.AspNetUser.UserName
                        };

            var output = JsonConvert.SerializeObject(chats.ToList());
            return Json(output, JsonRequestBehavior.AllowGet);
            }
        // GET: Chats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // GET: Chats/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,Message,Timestamp")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Chats.Add(chat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserId);
            return View(chat);
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserId);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserId,Message,Timestamp")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserId);
            return View(chat);
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chat chat = db.Chats.Find(id);
            db.Chats.Remove(chat);
            db.SaveChanges();
            return RedirectToAction("Index");
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
