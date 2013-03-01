using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblethon.Controller
{
    public interface IOfferLines
    {
        List<OfferLines> GetOfferLines();
    }
}