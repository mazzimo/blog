using Mazzimo.Models;
using Mazzimo.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mazzimo.Controllers
{
    public class HomeController : Controller
    {
        IPostRepository _postRepo;
        IResumeRepository _cvRepo;
        public HomeController(IPostRepository postRepo,
                              IResumeRepository cvRepo)
        {
            _postRepo = postRepo;
            _cvRepo = cvRepo;
        }

        public ActionResult Index()
        {
            var post = _postRepo.GetFirst();

            if (post == null)
                post = _postRepo.GetIntroductionPost();

            return View(post);
        }

        public ActionResult Cv(string id)
        {
            var cv = _cvRepo.GetResumeFromLanguageCode(id);
            if (cv == null)
                return HttpNotFound();
            ViewBag.Id = id;
            return View(cv);
        }

        public ActionResult CvPrint(string id)
        {
            var cv = _cvRepo.GetResumeFromLanguageCode(id);
            if (cv == null)
                return HttpNotFound();
            Response.Cache.SetExpires(DateTime.Now.AddYears(1));
            Response.Cache.SetCacheability(HttpCacheability.Public);

            return View(cv);
        }

        public ActionResult Post(string id)
        {
            var post = _postRepo.GetById(id);
            if (post == null)
                return HttpNotFound();

            return View(post);
        }
    }
}
