using Mazzimo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Mazzimo.Repositories
{
    public class FileResumeRespository : FileRepository<Cv>, IResumeRepository
    {
        public FileResumeRespository()
            : base("~/App_Data/resumes")
        { 
        }

        public Cv GetResumeFromLanguageCode(string languageCode)
        {
            return GetByRelativePath(languageCode + ".json");
        }

    }
}