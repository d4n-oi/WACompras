using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Compra
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public double Preco { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; }

        public bool Comprado { get; set; }

    public decimal Total => (decimal)Preco * Quantidade;

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCompra"] == null)
            {
                var lista = new List<Compra>
                {
                    new Compra { Nome = "Balinha", Preco = 1.90, Quantidade = 2, Categoria = "Comida" },
                    new Compra { Nome = "Frango", Preco = 19.90, Quantidade = 3, Categoria = "Comida" },
                    new Compra { Nome = "Paracetamal", Preco = 6.90, Quantidade = 2, Categoria = "Farmácia" },
                    new Compra { Nome = "Bolinha de borracha", Preco = 8.50, Quantidade = 1, Categoria = "Pet" },
                    new Compra { Nome = "Vestido", Preco = 100.00, Quantidade = 1, Categoria = "Vestuário" },
                    new Compra { Nome = "Abajur de pato", Preco = 28.90, Quantidade = 2, Categoria = "Variedades" },
                    new Compra { Nome = "Carrinho", Preco = 12.90, Quantidade = 5, Categoria = "Brinquedos" },
                    new Compra { Nome = "Roupa de abelinha", Preco = 26.90, Quantidade = 1, Categoria = "Pet" },
                    new Compra { Nome = "Bambolê", Preco = 11.90, Quantidade = 1, Categoria = "Brinquedos" },
                    new Compra { Nome = "Caneta", Preco = 1.70, Quantidade = 3, Categoria = "Variedades" }
                };

                // Definir ID automaticamente
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Id = i;
                }

                session["ListaCompra"] = lista;
            }
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaCompra"] as List<Compra>;
            this.Id = lista.Count;
            lista.Add(this);
        }

        public static Compra Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCompra"] as List<Compra>;
            return lista.FirstOrDefault(c => c.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCompra"] as List<Compra>;
            var original = lista.FirstOrDefault(c => c.Id == id);
            if (original != null)
            {
                original.Nome = this.Nome;
                original.Preco = this.Preco;
                original.Quantidade = this.Quantidade;
                original.Categoria = this.Categoria;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaCompra"] as List<Compra>;
            lista.RemoveAll(c => c.Id == this.Id);
        }
    }
}
