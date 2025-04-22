using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CompraController : Controller
    {
        // GET: Aluno
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            Compra.GerarLista(Session);

            return View(Session["ListaCompra"] as List<Compra>);
        }

        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCompra"] as List<Compra>).ElementAt(id));
        }
        public ActionResult Delete(int id)
        {
            return View((Session["ListaCompra"] as List<Compra>).ElementAt(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id, Compra compra)
        {
            Compra.Procurar(Session, id)?.Excluir(Session);
            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Compra());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compra compra)
        {
            compra.Adicionar(Session);

            return RedirectToAction("Listar");
        }

        public ActionResult Editar(int id)
        {
            return View(Compra.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Compra compra)
        {
            compra.Editar(Session, id);

            return RedirectToAction("Listar");
        }

    }
}