using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace delegados_t2_3.Controllers
{
    public class HomeController : Controller
    {
        private string division { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int A, int B)
        {
            delegados_t2_1.Operaciones operacion = new delegados_t2_1.Operaciones();
            operacion.MostrarMsj = EscribeResultado;
            float resultado = operacion.Divide(A, B);
            return Content(division);
        }

        private void EscribeResultado(string resultado)
        {
            division = resultado;
        }
    }
}