using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ControleEstoque.Web.Models
{
    public class PaisModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Paises.Count();
            }
            return ret;
        }

        //private static PaisModel MontarPais(SqlDataReader reader)
        //{
        //    return new PaisModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Codigo = (string)reader["codigo"],
        //        Ativo = (bool)reader["ativo"]
        //    };
        //}

        public static List<PaisModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<PaisModel>();

            using (var db = new ContextoBD())
            {                
                var parameters = new DynamicParameters();
                var sql = new StringBuilder("SELECT * FROM pais ");

                if (!string.IsNullOrEmpty(filtro))
                {
                    sql.Append(" WHERE LOWER(nome) LIKE @filtro");
                    parameters.Add("@filtro", $"'%{filtro.ToLower()}%'");
                }

                UtilBD.AppendOrdem(ref sql, ordem, "nome");

                if (pagina > 0 && tamPagina > 0)
                {
                    var pos = (pagina - 1) * tamPagina;
                    sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos > 0 ? pos - 1 : 0, tamPagina);
                }
                

                ret = db.Database.Connection.Query<PaisModel>(sql.ToString(), parameters).ToList();
            }

            return ret;
        }

        public static PaisModel RecuperarPeloId(int id)
        {
            PaisModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Paises.Find(id);
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
                    var pais = new PaisModel { Id = id };
                    db.Paises.Attach(pais);
                    db.Entry(pais).State = EntityState.Deleted;
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
                    db.Paises.Add(this);
                }
                else
                {
                    db.Paises.Attach(this);
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