using Mazzimo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Mazzimo.Repositories
{
    public class FilePostRepository :  FileRepository<Post>, IPostRepository
    {
        public FilePostRepository()
            : base("~/App_Data/posts")
        { 
        }

        public Post GetById(string id)
        {
            return GetByRelativePath(id + ".json");
        }

        public Post GetFirst()
        {
            var di = new DirectoryInfo(this.MappedBasePath);
            var paths = di.GetFiles()
                    .OrderByDescending(f => f.LastWriteTime)
                    .Select(f => f.Name);

            if (paths.Count() > 0)
                return GetByRelativePath(paths.First());
            else
                return null;
        }

        public Post GetIntroductionPost()
        {
            return new Post()
            {
                Title = "Mazzimo",
                Teaser = "Software Developer",
                ImageUrl = "/Images/cvimage_400x400.png"
            };
        }
    }
}