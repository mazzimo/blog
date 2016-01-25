using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Mazzimo.Repositories
{
    public abstract class FileRepository<T>
    {
        protected string BasePath
        { 
            get;
            private set;
        }

        protected string MappedBasePath
        {
            get 
            {
                return HttpContext.Current.Server.MapPath(this.BasePath);
            }
        }

        public FileRepository(string basePath)
        {
            this.BasePath = basePath + (basePath.EndsWith("/") ? "" : "/");
        }

        protected T GetByRelativePath(string path)
        {
            try
            {
                var settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTime };
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(this.MappedBasePath + path), settings);
            }
            catch (FileNotFoundException fnfex)
            {
                return default(T);
            }
        }
    }
}