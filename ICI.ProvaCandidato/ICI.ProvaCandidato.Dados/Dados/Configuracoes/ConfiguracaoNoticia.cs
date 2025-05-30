using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ICiProvaCandidato.Infra.Dados.Configuracoes
{
    public class ConfiguracaoNoticia : IEntityTypeConfiguration<Noticia>
    {
        public void Configure(EntityTypeBuilder<Noticia> builder)
        {

            
            builder.ToTable("Noticias");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .UseIdentityColumn();

            builder.Property(n => n.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Conteudo)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(n => n.DataPublicacao)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(n => n.Ativa)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(n => n.Titulo)
                .HasDatabaseName("IX_Noticias_Titulo");

            builder.HasIndex(n => n.DataPublicacao)
                .HasDatabaseName("IX_Noticias_DataPublicacao");
        }
    }
}
