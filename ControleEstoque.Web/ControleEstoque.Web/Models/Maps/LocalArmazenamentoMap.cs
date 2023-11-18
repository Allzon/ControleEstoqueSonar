﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class LocalArmazenamentoMap : EntityTypeConfiguration<LocalArmazenamentoModel>
    {
        public LocalArmazenamentoMap()
        {
            // Nome da tabela
            ToTable("tb_locaisArmazenamentos");

            // Chave primária e autoincremento
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            // Dando nome aos campos
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(50).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();
        }
    }
}