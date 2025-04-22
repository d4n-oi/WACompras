using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Compra
    {
        public string Nome { get; set; }
        public Double Preco { get; set; }
        public int Quantidade { get; set; }
        public string Categoria { get; set; }

        //publico significa que oura classe pode utilizar esse metodo

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCompra"] != null)
            {
                if(((List<Compra>)session["ListaCompra"]).Count > 0);
                {
                    return;
                }
                //tem uma lista, num preisa de ota naum, só tem que dar um jeito de puxar ela
            }
            var lista = new List<Compra>();
            lista.Add(new Compra {Nome = "Balinha", Preco = 1.90, Quantidade = 2, Categoria = "Comida"});
            lista.Add(new Compra {Nome = "Frango", Preco = 19.90, Quantidade = 3, Categoria = "Comida"});
            lista.Add(new Compra {Nome = "Paracetamal", Preco = 6.90, Quantidade = 2, Categoria = "Farmacia"});
            lista.Add(new Compra {Nome = "Bolinha de borrahca", Preco = 8.50, Quantidade = 1, Categoria = "Pet"});
            lista.Add(new Compra {Nome = "Vestido", Preco = 100.00, Quantidade = 1, Categoria = "Vestuario"});
            lista.Add(new Compra {Nome = "Abajur de pato", Preco = 28.90, Quantidade = 2, Categoria = "Variedades"});
            lista.Add(new Compra {Nome = "Carrinho", Preco = 12.90, Quantidade = 5, Categoria = "Brinquedos"});
            lista.Add(new Compra {Nome = "Roupa de abelinha", Preco = 26.90, Quantidade = 1, Categoria = "Pet"});
            lista.Add(new Compra {Nome = "Bambolê", Preco = 11.90, Quantidade = 1, Categoria = "Brinquedos"});
            lista.Add(new Compra {Nome = "Caneta", Preco = 1.70, Quantidade = 3, Categoria = "Variedades"});

            session.Remove("ListaCompra");
            session.Add("   ", lista);
        }
        public void Adicionar(HttpSessionStateBase session)
        {
            if(session ["ListaCompra"] != null)
            {
                (session["ListaCompra"] as List<Compra>).Add(this);
            }
        }
        public static Compra Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCompra"] != null)
            {
                return (session["ListaCompra"] as List<Compra>).ElementAt(id);
            }

            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCompra"] != null)
            {
                (session["ListaCompra"] as List<Compra>).Remove(this);
            }
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCompra"] != null)
            {
                var compra = Compra.Procurar(session, id);
                compra.Nome = this.Nome;
                compra.Preco = this.Preco;
                compra.Quantidade = this.Quantidade;    
                compra.Categoria = this.Categoria;
            }
        }

    }
}