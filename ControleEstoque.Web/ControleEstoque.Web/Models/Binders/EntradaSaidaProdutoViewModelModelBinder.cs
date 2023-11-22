using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace ControleEstoque.Web.Models
{
    public class EntradaSaidaProdutoViewModelModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valores = controllerContext.HttpContext.Request.Form;
            var ret = new EntradaSaidaProdutoViewModel() { Produtos = new Dictionary<int, int>() };

            try
            {
                ret.Data = DateTime.Parse(valores.Get("data"), new CultureInfo("en-US"));

                if (!string.IsNullOrEmpty(valores.Get("produtos")))
                {
                    var produtos = JsonConvert.DeserializeObject<List<dynamic>>(valores.Get("produtos"));

                    foreach (var produto in produtos)
                    {
                        ret.Produtos.Add((int)produto.IdProduto, (int)produto.Quantidade);
                    }
                };
            }
            catch
            {
            }

            return ret;
        }
    }
}