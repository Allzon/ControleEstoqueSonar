using System.Text;

namespace ControleEstoque.Web.Models
{
    public static class UtilBD
    {
        public static void AppendFiltro(ref StringBuilder sql)
        {            
            if (sql.ToString().Contains("WHERE"))            
                sql.Append(" AND (LOWER(c.nome) LIKE @filtro)");
            else            
                sql.Append(" WHERE LOWER(nome) LIKE @filtro");
        }

        public static void AppendOrdem(ref StringBuilder sql, string ordem, string def)
        {
            var order = !string.IsNullOrEmpty(ordem) ? ordem : def;
            sql.AppendFormat($" ORDER BY {order}");
        }
        
        public static void AppendPaginacao(ref StringBuilder sql, int pagina, int tamPagina)
        {
            if (pagina > 0 && tamPagina > 0)
            {
                var pos = (pagina - 1) * tamPagina;
                sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos > 0 ? pos - 1 : 0, tamPagina);
            }
        }
    }
}