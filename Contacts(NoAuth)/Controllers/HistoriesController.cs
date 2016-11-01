using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Contacts_NoAuth_.Models;

namespace Contacts_NoAuth_.Controllers
{
    public class HistoriesController : Controller
    {
        private Contacts_NoAuth_Context db = new Contacts_NoAuth_Context();

        // GET: Histories
        public async Task<ActionResult> Index()
        {
            return View(await db.Histories.ToListAsync());
        }

        // GET: Histories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = await db.Histories.FindAsync(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // GET: Histories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Histories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Time,Action,UserId")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Histories.Add(history);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(history);
        }

        // GET: Histories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = await db.Histories.FindAsync(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Time,Action,UserId")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(history);
        }

        // GET: Histories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = await db.Histories.FindAsync(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            History history = await db.Histories.FindAsync(id);
            db.Histories.Remove(history);
            await db.SaveChangesAsync();
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
