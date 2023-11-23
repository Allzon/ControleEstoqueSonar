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

        public static void AppendOrdem(ref StringBuilder sql, string ordem)
        {
            sql.AppendFormat(" ORDER BY {0}", !string.IsNullOrEmpty(ordem) ? ordem : "nome");
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