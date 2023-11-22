using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ControleEstoque.Web.Models
{
    public class UnidadeMedidaModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public bool Ativo { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.UnidadesMedidas.Count();
            }
            return ret;
        }

        //private static UnidadeMedidaModel MontarUnidadeMedida(SqlDataReader reader)
        //{
        //    return new UnidadeMedidaModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Sigla = (string)reader["sigla"],
        //        Ativo = (bool)reader["ativo"]
        //    };
        //}

        public static List<UnidadeMedidaModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<UnidadeMedidaModel>();

            using (var db = new ContextoBD())
            {                
                var parameters = new DynamicParameters();
                var sql = new StringBuilder("SELECT * FROM tb_unidadesMedidas ");
                if (!string.IsNullOrEmpty(filtro))
                {
                    sql.Append(" WHERE LOWER(nome) LIKE @filtro");
                    parameters.Add(@"filtro", $"'%{filtro.ToLower()}%'");
                }

                if (!string.IsNullOrEmpty(ordem))
                    sql.Append(" ORDER BY " + ordem);
                else
                    sql.Append(" ORDER BY c.nome");

                if (pagina > 0 && tamPagina > 0)
                {
                    var pos = (pagina - 1) * tamPagina;
                    sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos > 0 ? pos - 1 : 0, tamPagina);
                }
                
                ret = db.Database.Connection.Query<UnidadeMedidaModel>(sql.ToString(), parameters).ToList();
            }
            return ret;
        }

        public static UnidadeMedidaModel RecuperarPeloId(int id)
        {
            UnidadeMedidaModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.UnidadesMedidas.Find(id);
            }
            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var db = new ContextoBD())
                {
                    var unidadeMedida = new UnidadeMedidaModel { Id = id };
                    db.UnidadesMedidas.Attach(unidadeMedida);
                    db.Entry(unidadeMedida).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }
            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var db = new ContextoBD())
            {
                if (model == null)
                {
                    db.UnidadesMedidas.Add(this);
                }
                else
                {
                    db.UnidadesMedidas.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                }
                db.SaveChanges();
                ret = this.Id;
            }
            return ret;
        }
        #endregion
    }
}