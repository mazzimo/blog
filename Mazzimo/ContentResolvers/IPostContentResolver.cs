using Mazzimo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mazzimo.ContentResolvers
{
    public interface IPostContentResolver
    {
        string ExtractContent(Post post);
    }
}