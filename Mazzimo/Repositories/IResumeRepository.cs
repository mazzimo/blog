using Mazzimo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazzimo.Repositories
{
    public interface IResumeRepository
    {
        Cv GetResumeFromLanguageCode(string languageCode);
    }
}
