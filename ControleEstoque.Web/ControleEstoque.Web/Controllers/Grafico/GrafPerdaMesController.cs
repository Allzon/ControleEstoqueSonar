﻿using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers.Grafico
{
    public class GrafPerdaMesController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var mes = DateTime.Today.Month;
            var ano = DateTime.Today.Year;
            var quantidadeDias = DateTime.DaysInMonth(ano, mes);

            var dias = new List<int>();
            var perdas = new List<int>();

            for (int dia = 1; dia <= quantidadeDias; dia++)
            {
                dias.Add(dia);
                perdas.Add(0);
            }

            foreach (var perdaBd in ProdutoModel.PerdasNoMes(mes, ano))
            {
                perdas[perdaBd.Dia - 1] = perdaBd.Quant;
            }

            ViewBag.Dias = dias;
            ViewBag.Mes = mes;
            ViewBag.Ano = ano;
            ViewBag.Perdas = perdas;

            return View();
        }

        [Authorize]
        public ActionResult EntradaSaidaMes()
        {
            return View();
        }
    }
}