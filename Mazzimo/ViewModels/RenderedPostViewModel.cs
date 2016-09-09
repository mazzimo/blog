using Mazzimo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mazzimo.ViewModels
{
    public class RenderedPostViewModel
    {
        public Post Post { get; set; }
        public string TotalUrl { get; set; }
        public string RenderedContent { get; set; }
    }
}