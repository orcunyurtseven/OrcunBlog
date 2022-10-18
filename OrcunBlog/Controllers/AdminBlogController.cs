using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrcunBlog.Models;
using System.Web.Helpers;
using System.IO;

namespace OrcunBlog.Controllers
{
    public class AdminBlogController : Controller
    {
        BloggerEntities db = new BloggerEntities();
        // GET: AdminBlog
        public ActionResult Index()
        {
            var blog = db.Blogs.ToList();
            return View(blog);
        }

        // GET: AdminBlog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();

            }

            return View(blog);
        }

        // GET: AdminBlog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminBlog/Create
        [HttpPost]
        public ActionResult Create(Blog blog, HttpPostedFile Foto)
        {
            try
            {
                if (Foto != null)
                {
                    WebImage webImage = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    webImage.Resize(800, 350);
                    webImage.Save("~/Uploads/" + newfoto);
                    blog.Foto = "/Uploads/" + newfoto;
                }
                blog.BlogOkunmaSayisi = 0;
                blog.BlogTarih = DateTime.Now;
                db.Blogs.Add(blog);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminBlog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: AdminBlog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase Foto, Blog blog)
        {
            try
            {
                var blogguncelle = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
                if (Foto != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(blogguncelle.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(blogguncelle.Foto)); 

                    }

                    WebImage webImage = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    webImage.Resize(800, 350);
                    webImage.Save("~/Uploads/" + newfoto);
                    blogguncelle.Foto = "/Uploads/" + newfoto;
                    blogguncelle.BlogBaslik = blog.BlogBaslik;
                    blogguncelle.BlogIcerik=blog.BlogIcerik;
                    blogguncelle.BlogOkunmaSayisi=blog.BlogOkunmaSayisi;
                    blogguncelle.BlogOkunmaSuresi=blog.BlogOkunmaSuresi;
                    db.SaveChanges();
                    return RedirectToAction("Index");  

                }

                return View();
            }
            catch
            {
                return View();
            }
        }


        // GET: AdminBlog/Delete/5
        public ActionResult Delete(int id)
        {
            var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: AdminBlog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
                if (blog == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(blog.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(blog.Foto));
                }
                db.Blogs.Remove(blog);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
