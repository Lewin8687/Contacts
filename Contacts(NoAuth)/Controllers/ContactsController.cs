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
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Core.Objects;

namespace Contacts_NoAuth_.Controllers
{
    public class ContactsController : Controller
    {
        private Contacts_NoAuth_Context db = new Contacts_NoAuth_Context();
        private static int currentPage = 1;
        private static int pageSize = 3;
        private static string currentSearch = "";

        // GET: Contacts
        public ActionResult Index(string search, string sortBy, int? page, int? PageSizeOptions)
        {
            // Three options of page size
            ViewBag.PageSizeOptions = new List<SelectListItem> {
                new SelectListItem { Value = "3", Text = "3", Selected = 3 == pageSize},
                new SelectListItem { Value = "5", Text = "5", Selected = 5 == pageSize },
                new SelectListItem { Value = "10", Text = "10", Selected = 10 == pageSize }
            };

            ViewBag.SortNameParam = string.IsNullOrEmpty(sortBy) ? "NameDesc" : "";
            ViewBag.SortDeletedParam = sortBy == "Deleted" ? "DeletedDesc" : "Deleted";
            
            // Check whether should goto page 1
            if ((search != null && search != currentSearch) || (PageSizeOptions != null && PageSizeOptions != pageSize))
            {
                currentPage = 1;
            }

            // Update current status
            currentPage = page ?? currentPage;
            pageSize = PageSizeOptions ?? pageSize;
            currentSearch = search ?? currentSearch;

            var contacts = db.Contacts.Where(e => e.FirstName.Contains(currentSearch) || currentSearch == null);

            // Sort
            switch (sortBy)
            {
                case "NameDesc":
                    contacts = contacts.OrderByDescending(x => x.FirstName);
                    break;
                case "Deleted":
                    contacts = contacts.OrderByDescending(x => x.IsDeleted);
                    break;
                case "DeletedAsc":
                    contacts = contacts.OrderBy(x => x.IsDeleted);
                    break;
                default:
                    contacts = contacts.OrderBy(x => x.IsDeleted);
                    break;
            }
            return View(contacts.ToList().ToPagedList(currentPage, pageSize));
        }

        // GET: ShowDetails_5
        [Route("ShowDetails_{id}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new ContactDetails();
            model.Contact = await db.Contacts.FindAsync(id);
            model.History = await db.Histories.Where(h => h.UserId == id).OrderByDescending(h => h.Time).ToListAsync();
            if (model.Contact == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: CreateContact
        [Route("CreateContact")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CreateContact")]
        public async Task<ActionResult> Create([Bind(Include = "ID,FirstName,LastName,Email,Phone,Postal,Address,IsDeleted")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                // Create a history
                var history = new History();
                history.Action = "Created";
                
                // Upper case for postal code
                contacts.Postal = contacts.Postal.ToUpper();
                db.Contacts.Add(contacts);
                
                await db.SaveChangesAsync();
                history.UserId = contacts.ID;
                history.Time = DateTime.Now;
                db.Histories.Add(history);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contacts);
        }

        // GET: EditContact_5
        [Route("EditContact_{id}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // POST: EditContact_5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditContact_{id}")]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Phone,Postal,Address,IsDeleted")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                contacts.Postal = contacts.Postal.ToUpper();
                db.Contacts.Attach(contacts);
                var entry = db.Entry(contacts);
                foreach (var propertyName in entry.OriginalValues.PropertyNames)
                {
                    var original = entry.GetDatabaseValues().GetValue<object>(propertyName);
                    var current = entry.CurrentValues.GetValue<object>(propertyName);
                    if (!object.Equals(original, current))
                    {
                        entry.Property(propertyName).IsModified = true;
                        // Create history for each modified property
                        var history = new History
                        {
                            UserId = contacts.ID,
                            Time = DateTime.Now,
                            Action = propertyName + ": modified from " + (string.IsNullOrEmpty(original.ToString()) ? original.ToString(): "") + 
                            " to " + (string.IsNullOrEmpty(current.ToString()) ? original.ToString() : "")
                        };
                        db.Histories.Add(history);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contacts);
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Create a history
            var history = new History();
            history.Action = "Deleted";
            history.UserId = id;
            history.Time = DateTime.Now;
            db.Histories.Add(history);

            var contacts = await db.Contacts.FindAsync(id);
            // Soft delete record
            contacts.IsDeleted = true;
            db.Entry(contacts).State = EntityState.Modified;
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
