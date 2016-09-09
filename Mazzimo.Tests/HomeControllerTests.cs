using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mazzimo.Controllers;
using Moq;
using Mazzimo.Repositories;
using Mazzimo.Models;
using System.Web.Mvc;
using Shouldly;

namespace Mazzimo.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<IPostRepository> _postRepo;
        private Mock<IResumeRepository> _cvRepo;

        [TestInitialize]
        public void Setup()
        {
            _postRepo = new Mock<IPostRepository>();
            _cvRepo = new Mock<IResumeRepository>();
            _controller = new HomeController(_postRepo.Object, _cvRepo.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _postRepo = null;
            _cvRepo = null;
            _controller = null;
        }

        [TestMethod]
        public void IndexShouldReturnIntroductionPostIfFirstPostIsNull()
        {
            var introPost = new Post();
            _postRepo.Setup(r => r.GetFirst()).Returns((Post)null); 
            _postRepo.Setup(r => r.GetIntroductionPost()).Returns(introPost);

            var result = _controller.Index() as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<Post>();

            var postResult = result.ViewData.Model as Post;

            introPost.ShouldBe(postResult);


        }

        [TestMethod]
        public void IndexShouldReturnFirstPostIfNotNull()
        {
            var firstPost = new Post();
            _postRepo.Setup(r => r.GetFirst()).Returns(firstPost);

            var result = _controller.Index() as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<Post>();

            var postResult = result.ViewData.Model as Post;

            firstPost.ShouldBe(postResult);

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

            var result = _controller.Post("test") as ViewResult;

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType<Post>();

            var postResult = result.ViewData.Model as Post;

            foundPost.ShouldBe(postResult);

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
