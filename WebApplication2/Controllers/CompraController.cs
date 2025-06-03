using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace WebApplication2.Controllers
{
    public class CompraController : Controller
    {
        public ActionResult Index() => RedirectToAction("Listar");

        public ActionResult Listar()
        {
            Compra.GerarLista(Session);
            return View(Session["ListaCompra"] as List<Compra>);
        }

        public ActionResult Exibir(int id)
        {
            var compra = Compra.Procurar(Session, id);
            if (compra == null)
                return HttpNotFound("Compra não encontrada");

            return View(compra);
        }

        public ActionResult Create()
        {
            return View(new Compra());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compra compra)
        {
            if (ModelState.IsValid)
            {
                compra.Adicionar(Session);
                return RedirectToAction("Listar");
            }

            return View(compra);
        }

        public ActionResult Editar(int id)
        {
            var compra = Compra.Procurar(Session, id);
            if (compra == null)
                return HttpNotFound("Compra não encontrada");

            return View(compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Compra compra)
        {
            if (ModelState.IsValid)
            {
                compra.Editar(Session, id);
                return RedirectToAction("Listar");
            }

            return View(compra);
        }

        public ActionResult Delete(int id)
        {
            var compra = Compra.Procurar(Session, id);
            if (compra == null)
                return HttpNotFound("Compra não encontrada");

            return View(compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var compra = Compra.Procurar(Session, id);
            if (compra != null)
                compra.Excluir(Session);

            return RedirectToAction("Listar");
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var compra = Compra.Procurar(Session, id);
            if (compra != null)
            {
                compra.Excluir(Session);
                return Json(new { sucesso = true });
            }

            return new HttpStatusCodeResult(404, "Compra não encontrada");
        }

        [HttpPost]
        public ActionResult ToggleCompradoAjax(int id, string filtro = "")
        {
            var lista = Session["ListaCompra"] as List<Compra>;
            var item = lista.FirstOrDefault(c => c.Id == id);

            if (item == null)
                return Json(new { sucesso = false });

            item.Comprado = !item.Comprado;

            var itensFiltrados = string.IsNullOrEmpty(filtro)
                ? lista
                : lista.Where(x => x.Categoria == filtro);

            decimal total = itensFiltrados
                .Where(c => !c.Comprado)
                .Sum(c => (decimal)c.Preco * c.Quantidade);

            return Json(new
            {
                sucesso = true,
                totalAtualizado = total.ToString("F2")
            });
        }


        [HttpGet]
        public ActionResult DownloadPdf(string filtro = "", string total = "")
        {
            var compras = Session["ListaCompra"] as List<Compra>;
            if (compras == null || !compras.Any())
                return new HttpStatusCodeResult(400, "Nenhuma compra encontrada");

            if (!string.IsNullOrEmpty(filtro))
                compras = compras
                    .Where(x => x.Categoria.Trim().ToLower() == filtro.Trim().ToLower())
                    .ToList();

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, ms).CloseStream = false;
                doc.Open();

                doc.Add(new Paragraph("Lista de Compras - Categoria: " + (string.IsNullOrEmpty(filtro) ? "Todas" : filtro)));
                doc.Add(new Paragraph(" ")); 
                

                PdfPTable table = new PdfPTable(5);
                table.AddCell("✓");
                table.AddCell("Nome");
                table.AddCell("Preço");
                table.AddCell("Qtd");
                table.AddCell("Categoria");

                foreach (var item in compras)
                {
                    table.AddCell(item.Comprado ? "X" : "");
                    table.AddCell(item.Nome);
                    table.AddCell(item.Preco.ToString("F2"));
                    table.AddCell(item.Quantidade.ToString());
                    table.AddCell(item.Categoria);
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Preço total: R$ " + total));
                doc.Close();
                ms.Position = 0;

                return File(ms.ToArray(), "application/pdf", "lista-compras.pdf");
            }
        }



    }
}
