using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Mazzimo.Models
{

    public class Post
    {
        public string Code { get;set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string ImageUrl { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public List<string> Tags { get; set; }
        public List<Post> NextPosts { get; set; }
        public DateTime PostDate { get; set; }
    }

}