using Mazzimo.ContentResolvers;
using Mazzimo.Factories;
using Mazzimo.Models;
using Mazzimo.Repositories;
using Mazzimo.ViewModels;
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
        IContextFactory _ctxFactory;
        IPostContentResolver _postContentResolver;

        public HomeController(IPostRepository postRepo,
                              IResumeRepository cvRepo,
                              IContextFactory ctxFactory,
                              IPostContentResolver postContentResolver)
        {
            _postRepo = postRepo;
            _cvRepo = cvRepo;
            _ctxFactory = ctxFactory;
            _postContentResolver = postContentResolver;
        }

        public ActionResult Index()
        {
            var post = _postRepo.GetFirst();

            if (post == null)
                post = _postRepo.GetIntroductionPost();

            var viewModel = GetPostViewModel(post);

            return View(viewModel);
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

            return View(cv);
        }

        public ActionResult Post(string id)
        {
            var post = _postRepo.GetById(id);

            if (post == null)
                return HttpNotFound();

            var viewModel = GetPostViewModel(post);

            return View(viewModel);
        }

        private RenderedPostViewModel GetPostViewModel(Post post)
        {
            var currentUri = _ctxFactory.GetCurrentRequestUri();

            var result = new RenderedPostViewModel()
            {
                Post = post,
                RenderedContent = _postContentResolver.ExtractContent(post),
                TotalUrl = GetTotalUrl(currentUri)
            };

            if (String.IsNullOrEmpty(result.Post.ImageUrl))
            {
                post.ImageUrl = GetBaseUrl(currentUri) + "/Images/cvimage_400x400.png";
                post.ImageHeight = 400;
                post.ImageWidth = 400;
            }

            return result;

        }

        private string GetBaseUrl(Uri uri)
        { 
            return uri.Scheme + "://" + uri.Authority;
        }

        private string GetTotalUrl(Uri uri)
        {
            return String.Format("{0}{1}{2}{3}", uri.Scheme, Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath);
        }
    }
}
