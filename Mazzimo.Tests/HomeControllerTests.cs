using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mazzimo.Controllers;
using Moq;
using Mazzimo.Repositories;
using Mazzimo.Models;
using System.Web.Mvc;
using Shouldly;
using Mazzimo.Factories;
using Mazzimo.ContentResolvers;
using Mazzimo.ViewModels;

namespace Mazzimo.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private const string TEST_RENDERED_CONTENT = "<h1>Test Rendered Content</h1>";
        private readonly Uri TEST_URI = new Uri("http://mazzimo.fr/abc");
        private HomeController _controller;
        private Mock<IPostRepository> _postRepo;
        private Mock<IResumeRepository> _cvRepo;
        private Mock<IContextFactory> _ctxFactory;
        private Mock<IPostContentResolver> _postContentResolver;

        [TestInitialize]
        public void Setup()
        {
            _postRepo = new Mock<IPostRepository>();
            _cvRepo = new Mock<IResumeRepository>();
            _ctxFactory = new Mock<IContextFactory>();
            _postContentResolver = new Mock<IPostContentResolver>();
            _controller = new HomeController(_postRepo.Object, _cvRepo.Object, _ctxFactory.Object, _postContentResolver.Object);
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            _postRepo = null;
            _cvRepo = null;
            _ctxFactory = null;
            _postContentResolver = null;
            _controller = null;
        }

        [TestMethod]
        public void IndexShouldReturnIntroductionPostIfFirstPostIsNull()
        {
            var introPost = new Post();
            _postRepo.Setup(r => r.GetFirst()).Returns((Post)null); 
            
            _postRepo.Setup(r => r.GetIntroductionPost())
                     .Returns(introPost);

            _postContentResolver.Setup(r => r.ExtractContent(It.IsAny<Post>()))
                                .Returns(TEST_RENDERED_CONTENT);

            _ctxFactory.Setup(f => f.GetCurrentRequestUri())
                       .Returns(TEST_URI);

            var result = _controller.Index() as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<RenderedPostViewModel>();

            var postResult = result.ViewData.Model as RenderedPostViewModel;
            var expectedTotalUrl = String.Format("{0}{1}{2}{3}", TEST_URI.Scheme, Uri.SchemeDelimiter, TEST_URI.Authority, TEST_URI.AbsolutePath);
            
            postResult.Post.ShouldBe(introPost);
            postResult.RenderedContent.ShouldBe(TEST_RENDERED_CONTENT);
            postResult.TotalUrl.ShouldBe(expectedTotalUrl); 

        }

        [TestMethod]
        public void IndexShouldReturnFirstPostIfNotNull()
        {
            var firstPost = new Post();
            _postRepo.Setup(r => r.GetFirst()).Returns(firstPost);

            _postContentResolver.Setup(r => r.ExtractContent(It.IsAny<Post>()))
                    .Returns(TEST_RENDERED_CONTENT);

            _ctxFactory.Setup(f => f.GetCurrentRequestUri())
                       .Returns(TEST_URI);

            var result = _controller.Index() as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<RenderedPostViewModel>();

            var postResult = result.ViewData.Model as RenderedPostViewModel;
            var expectedTotalUrl = String.Format("{0}{1}{2}{3}", TEST_URI.Scheme, Uri.SchemeDelimiter, TEST_URI.Authority, TEST_URI.AbsolutePath);

            postResult.Post.ShouldBe(firstPost);
            postResult.RenderedContent.ShouldBe(TEST_RENDERED_CONTENT);
            postResult.TotalUrl.ShouldBe(expectedTotalUrl); 

        }

        [TestMethod]
        public void CvShouldReturnIfFound()
        {
            var foundCv = new Cv();
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns(foundCv);

            var result = _controller.Cv("test") as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<Cv>();

            var cvResult = result.ViewData.Model as Cv;

            foundCv.ShouldBe(cvResult);
        }

        [TestMethod]
        public void Cv404()
        {
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns((Cv)null);

            var result = _controller.Cv("test");

            result.ShouldBeOfType<HttpNotFoundResult>();
        }

        [TestMethod]
        public void CvPdfShouldReturnIfFound()
        {
            var foundCv = new Cv();
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns(foundCv);

            var result = _controller.CvPrint("test") as ViewResult;

            result.ShouldNotBeNull();
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<Cv>();

            var cvResult = result.ViewData.Model as Cv;

            foundCv.ShouldBe(cvResult);

        }

        [TestMethod]
        public void CvPdf404()
        {
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns((Cv)null);
            var result = _controller.CvPrint("test");

            result.ShouldBeOfType<HttpNotFoundResult>();

        }



        [TestMethod]
        public void PostShouldReturnIfFound()
        {
            var foundPost = new Post();
            _postRepo.Setup(r => r.GetById(It.IsAny<string>())).Returns(foundPost);

            _postContentResolver.Setup(r => r.ExtractContent(It.IsAny<Post>()))
                                .Returns(TEST_RENDERED_CONTENT);

            _ctxFactory.Setup(f => f.GetCurrentRequestUri())
                       .Returns(TEST_URI);

            var result = _controller.Post("test") as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<RenderedPostViewModel>();

            var postResult = result.ViewData.Model as RenderedPostViewModel;

            var expectedTotalUrl = String.Format("{0}{1}{2}{3}", TEST_URI.Scheme, Uri.SchemeDelimiter, TEST_URI.Authority, TEST_URI.AbsolutePath);

            postResult.Post.ShouldBe(foundPost);
            postResult.RenderedContent.ShouldBe(TEST_RENDERED_CONTENT);
            postResult.TotalUrl.ShouldBe(expectedTotalUrl); 


        }

        [TestMethod]
        public void Post404()
        {
            _postRepo.Setup(r => r.GetById(It.IsAny<string>())).Returns((Post)null);
            var result = _controller.Post("test");

            result.ShouldBeOfType<HttpNotFoundResult>();
        }

    }
}
