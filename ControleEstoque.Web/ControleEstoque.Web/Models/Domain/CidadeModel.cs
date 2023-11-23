﻿using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ControleEstoque.Web.Models
{
    public class CidadeModel
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int IdEstado { get; set; }
        public virtual EstadoModel Estado { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;
            using (var db = new ContextoBD())
            {
                ret = db.Cidades.Count();
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    ret = conexao.ExecuteScalar<int>("SELECT COUNT (*) FROM cidade");
            //    //using (var comando = new SqlCommand())
            //    //{
            //    //    comando.Connection = conexao;
            //    //    comando.CommandText = "SELECT COUNT (*) FROM cidade";

            //    //    ret = (int)comando.ExecuteScalar();
            //    //}
            //}
        }

        //private static CidadeModel MontarCidade(SqlDataReader reader)
        //{
        //    return new CidadeModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Ativo = (bool)reader["ativo"],
        //        IdEstado = (int)reader["id_estado"]
        //    };
        //}

        public static List<CidadeViewModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", int idEstado = 0)
        {
            var ret = new List<CidadeViewModel>();

            using (var db = new ContextoBD())
            {
                var parameters = new DynamicParameters();
                var sql = new StringBuilder("SELECT c.id, c.nome, c.ativo, c.id_estado as IdEstado, e.id_pais as IdPais, e.nome as NomeEstado, p.nome as NomePais FROM cidade c, estado e, pais p WHERE (c.id_estado = e.id) AND (e.id_pais = p.id)");
                
                if (!string.IsNullOrEmpty(filtro))
                {
                    UtilBD.AppendFiltro(ref sql);
                    parameters.Add("@filtro", $"%{filtro.ToLower()}%");
                }
                
                if (idEstado > 0)
                {
                    sql.Append(" AND (id_estado = @Id_Estado)");
                    parameters.Add("@Id_Estado", idEstado);
                }

                sql.AppendFormat(" ORDER BY {0}", !string.IsNullOrEmpty(ordem) ? ordem : "c.nome");
                UtilBD.AppendPaginacao(ref sql, pagina, tamPagina);

                ret = db.Database.Connection.Query<CidadeViewModel>(sql.ToString(), parameters).ToList();
            }
            return ret;
        }

        public static CidadeViewModel RecuperarPeloId(int id)
        {
            CidadeViewModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Cidades
                    .Include(x => x.Estado)
                    .Where(x => x.Id == id)
                    .Select(x => new CidadeViewModel
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Ativo = x.Ativo,
                        IdEstado = x.IdEstado,
                        IdPais = x.Estado.IdPais
                    })
                    .SingleOrDefault();
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    var sql = "select c.*, e.id_pais from cidade c, estado e where (c.id = @id) and (c.id_estado = e.id)";
            //    var parametros = new { id };
            //    ret = conexao.Query<CidadeModel>(sql, parametros).SingleOrDefault();
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "select c.*, e.id_pais from cidade c, estado e where (c.id = @id) and (c.id_estado = e.id)";

            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

            //    var reader = comando.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        ret = MontarCidade(reader);
            //    }
            //}
            //}
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var db = new ContextoBD())
                {
                    var cidade = new CidadeModel { Id = id };
                    db.Cidades.Attach(cidade);
                    db.Entry(cidade).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
                //using (var conexao = new SqlConnection())
                //{
                //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                //    conexao.Open();
                //    var sql = "DELETE FROM cidade WHERE (id = @id)";
                //    var parametros = new { id };
                //    ret = (conexao.Execute(sql, parametros) > 0);
                //using (var comando = new SqlCommand())
                //{
                //    comando.Connection = conexao;
                //    comando.CommandText = "DELETE FROM cidade WHERE (id = @id)";

                //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                //    ret = (comando.ExecuteNonQuery() > 0);
                //}
                //}
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
                    db.Cidades.Add(this);
                }
                else
                {
                    db.Cidades.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                }
                db.SaveChanges();
                ret = this.Id;
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    if (model == null)
            //    {
            //        var sql = "INSERT INTO cidade (nome, ativo, id_estado) VALUES (@nome, @ativo, @id_estado); SELECT CONVERT(INT, SCOPE_IDENTITY())";
            //        var parametros = new { nome = this.Nome, ativo = (this.Ativo ? 1 : 0), id_estado = this.IdEstado };
            //        ret = conexao.ExecuteScalar<int>(sql, parametros);
            //    }
            //    else
            //    {
            //        var sql = "UPDATE cidade SET nome = @nome, ativo=@ativo, id_estado=@id_estado WHERE id = @id";
            //        var parametros = new { nome = this.Nome, ativo = (this.Ativo ? 1 : 0), id_estado = this.IdEstado, id = this.Id };
            //        if (conexao.Execute(sql, parametros) > 0)
            //        {
            //            ret = this.Id;
            //        }
            //    }
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;

            //    if (model == null)
            //    {
            //        comando.CommandText = "INSERT INTO cidade (nome, ativo, id_estado) VALUES (@nome, @ativo, @id_estado); SELECT CONVERT(INT, SCOPE_IDENTITY())";
            //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
            //        comando.Parameters.Add("@id_estado", SqlDbType.Int).Value = this.IdEstado;
            //        ret = (int)comando.ExecuteScalar();
            //    }
            //    else
            //    {
            //        comando.CommandText = "UPDATE cidade SET nome = @nome, ativo=@ativo, id_estado=@id_estado WHERE id = @id";

            //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
            //        comando.Parameters.Add("@id_estado", SqlDbType.Int).Value = this.IdEstado;
            //        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

            //        if (comando.ExecuteNonQuery() > 0)
            //        {
            //            ret = this.Id;
            //        }
            //    }
            //}
        }
        #endregion
    }
}