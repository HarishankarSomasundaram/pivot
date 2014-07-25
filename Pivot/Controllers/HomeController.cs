using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Pivot.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Demo()
        {
            return View();
        }

        Pivot.Models.NWindEntities db = new Pivot.Models.NWindEntities();



        [ValidateInput(false)]
        public ActionResult PivotGrid2Partial()
        {
            var model = db.Invoices;
            return PartialView("_PivotGrid2Partial", model.ToList());
        }

        [ValidateInput(false)]
        public ActionResult PivotGrid3Partial()
        {
            var model = new object[0];
            return PartialView("_PivotGrid3Partial", model);
        }
	}
}