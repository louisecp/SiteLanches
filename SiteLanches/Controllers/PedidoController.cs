﻿using Microsoft.AspNetCore.Mvc;
using SiteLanches.Models;
using SiteLanches.Repositories.Interfaces;

namespace SiteLanches.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {
            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;

            //obtem os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            //verifica se exixtem itens de pedido
            if(_carrinhoCompra.CarrinhoCompraItens.Count == 0) 
            {
                ModelState.AddModelError("", "Seu carrinho está vazio, que tal incluir um lanche...");
            }

            //calcula o total de itens e o total do pedido
            foreach(var item in itens)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

            //valida os dados do pedido
            if(ModelState.IsValid)
            {
                //Cria o pedido e os detalhes
                _pedidoRepository.CriarPedido(pedido);

                //define mensagens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido";
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                //limpa o carrinho
                _carrinhoCompra.LimparCarrinho();

                //exibe a view com dados do cliente e do pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
        }
        public IActionResult CheckoutCompleto()
        {
            ViewBag.Cliente = TempData["Cliente"];
            ViewBag.DataPedido = TempData["DataPedido"];
            ViewBag.NumeroPedido = TempData["NumeroPedido"];
            ViewBag.TotalPedido = TempData["TotalPedido"];
            ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :) ";
            return View();
        }


    }
}
