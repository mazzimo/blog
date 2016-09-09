using CommonMark;
using Mazzimo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Mazzimo.ContentResolvers
{
    public class CommonMarkPostContentResolver : IPostContentResolver
    {
        public string ExtractContent(Post post)
        {
            var fileToRead = HttpContext.Current.Server.MapPath(post.ContentFile);
            var fileContent = File.ReadAllText(fileToRead);
            var parsedFileContent = CommonMarkConverter.Convert(fileContent);
            var result = AddAboutBlankToLinks(parsedFileContent);
            return result;
        }

        private string AddAboutBlankToLinks(string inputContentString)
        {
            return inputContentString.Replace("href=", "target='_blank' href=");
        }
    }
}