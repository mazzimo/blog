using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mazzimo.Controllers;
using Moq;
using Mazzimo.Repositories;
using Mazzimo.Models;
using System.Web.Mvc;

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

            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Post));

            var postResult = result.ViewData.Model as Post;

            Assert.AreEqual(introPost, postResult); 

        }

        [TestMethod]
        public void IndexShouldReturnFirstPostIfNotNull()
        {
            var firstPost = new Post();
            _postRepo.Setup(r => r.GetFirst()).Returns(firstPost);

            var result = _controller.Index() as ViewResult;
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Post));

            var postResult = result.ViewData.Model as Post;

            Assert.AreEqual(firstPost, postResult); 

        }

        [TestMethod]
        public void CvShouldReturnIfFound()
        {
            var foundCv = new Cv();
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns(foundCv);

            var result = _controller.Cv("test") as ViewResult;
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Cv));

            var cvResult = result.ViewData.Model as Cv;

            Assert.AreEqual(foundCv, cvResult); 
        }

        [TestMethod]
        public void Cv404()
        {
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns((Cv)null);
            var result = _controller.Cv("test");
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void CvPdfShouldReturnIfFound()
        {
            var foundCv = new Cv();
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns(foundCv);

            var result = _controller.CvPdf("test") as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Cv));

            var cvResult = result.ViewData.Model as Cv;

            Assert.AreEqual(foundCv, cvResult);
        }

        [TestMethod]
        public void CvPdf404()
        {
            _cvRepo.Setup(r => r.GetResumeFromLanguageCode(It.IsAny<string>())).Returns((Cv)null);
            var result = _controller.CvPdf("test");
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }



        [TestMethod]
        public void PostShouldReturnIfFound()
        {
            var foundPost = new Post();
            _postRepo.Setup(r => r.GetById(It.IsAny<string>())).Returns(foundPost);

            var result = _controller.Post("test") as ViewResult;
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Post));

            var postResult = result.ViewData.Model as Post;

            Assert.AreEqual(foundPost, postResult); 
        }

        [TestMethod]
        public void Post404()
        {
            _postRepo.Setup(r => r.GetById(It.IsAny<string>())).Returns((Post)null);
            var result = _controller.Post("test");
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

    }
}
