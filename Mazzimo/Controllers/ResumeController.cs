using Mazzimo.Models;
using Mazzimo.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Mazzimo.Controllers
{
    public class ResumeController : ApiController
    {
        IResumeRepository _cvRepo;
        public ResumeController(IResumeRepository cvRepo)
        {
            _cvRepo = cvRepo;
        }

        public Cv Get(string id)
        {
            return _cvRepo.GetResumeFromLanguageCode(id);
        }

    }
}