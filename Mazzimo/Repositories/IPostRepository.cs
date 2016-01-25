using Mazzimo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazzimo.Repositories
{
    public interface IPostRepository
    {
        Post GetById(string id);
        Post GetFirst();
        Post GetIntroductionPost();
    }
}
