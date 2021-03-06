﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using system_oceny.Models;

namespace system_oceny.Controllers
{
    public class FirmyController : Controller
    {
        private FirmaDBCtxt db = new FirmaDBCtxt();

        public ActionResult Index(string sortowanie, FirmaSzukaj Model)
        {
            ViewBag.SortedBy = sortowanie;
            ViewBag.SortByOcena = sortowanie == null ? "Ocena_Malejaco" : "";
            ViewBag.SortByNazwa = sortowanie == "Nazwa_Malejaco" ? "Nazwa_Rosnaco" : "Nazwa_Malejaco";
            ViewBag.SortByBranza = sortowanie == "Branza_Malejaco" ? "Branza_Rosnaco" : "Branza_Malejaco";
            var firmy = from i in db.Firmy
                        select i;

             //Wyszukiwanie
            if (ModelState.IsValid)
            {
                if (Model.BranzaZnajdz != null && Model.NazwaZnajdz != null)
                {
                    firmy = from i in db.Firmy
                            where i.Nazwa.Contains(Model.NazwaZnajdz) && i.Branza.Contains(Model.BranzaZnajdz)
                            select i;
                }
                else if (Model.BranzaZnajdz != null)
                {
                    firmy = from i in db.Firmy
                            where i.Branza.Contains(Model.BranzaZnajdz)
                            select i;
                }
                else if (Model.NazwaZnajdz != null)
                {
                    firmy = from i in db.Firmy
                            where i.Nazwa.Contains(Model.NazwaZnajdz)
                            select i;
                }
                ViewBag.BranzaZnajdz = Model.BranzaZnajdz;
                ViewBag.NazwaZnajdz = Model.NazwaZnajdz;
            }


            //Sortowanie
            switch (sortowanie)
            {
                case "Nazwa_Malejaco":
                    firmy = firmy.OrderByDescending(s => s.Nazwa);
                    break;
                case "Nazwa_Rosnaco":
                    firmy = firmy.OrderBy(s => s.Nazwa);
                    break;
                case "Branza_Malejaco":
                    firmy = firmy.OrderByDescending(s => s.Branza);
                    break;
                case "Branza_Rosnaco":
                    firmy = firmy.OrderBy(s => s.Branza);
                    break;
                case "Ocena_Malejaco":
                    firmy = firmy.OrderBy(s => s.ocena);
                    break;
                default:
                    firmy = firmy.OrderByDescending(s => s.ocena);
                    break;
            }
            return View(firmy.ToList());
        }

        // GET: /Firmy/Details/X
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firma firma = db.Firmy.Find(id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            return View(firma);
        }

        //Powinno się wywoływać po wywołaniu oceny
        [HttpPost]
        public ActionResult Details(int? id, string FirmaOcen)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firma firma = db.Firmy.Find(id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            db.Entry(firma).State = EntityState.Modified;  
            firma.ocena = (firma.ocena*firma.ilosc_ocen+Single.Parse(FirmaOcen))/(firma.ilosc_ocen+1);
            firma.ilosc_ocen++;
            db.SaveChanges();
            return View(firma);
        }

        // GET: /Firmy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Firmy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Branza,Nazwa,Opis,NIP, email, miasto, kod_pocztowy, adres,numer_telefonu")] Firma firma)
        {
            firma.ocena = 0;
            firma.ilosc_ocen = 0;
            if (ModelState.IsValid)
            {
                db.Firmy.Add(firma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(firma);
        }

        // GET: /Firmy/Edit/X
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firma firma = db.Firmy.Find(id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            return View(firma);
        }

        // POST: /Firmy/Edit/X
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Branza,Nazwa,Opis,NIP, email, miasto, kod_pocztowy, adres, numer_telefonu")] Firma firma)
        {
            if (ModelState.IsValid)
            {
                db.Entry(firma).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(firma);
        }

        // GET: /Firmy/Delete/X
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firma firma = db.Firmy.Find(id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            return View(firma);
        }

        // POST: /Firmy/Delete/X
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Firma firma = db.Firmy.Find(id);
            db.Firmy.Remove(firma);
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
