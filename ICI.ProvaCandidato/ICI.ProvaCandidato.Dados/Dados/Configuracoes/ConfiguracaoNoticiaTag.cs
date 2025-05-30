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
    public class ConfiguracaoNoticiaTag : IEntityTypeConfiguration<NoticiaTag>
    {
        public void Configure(EntityTypeBuilder<NoticiaTag> builder)
        {
            builder.ToTable("NoticiaTags");

            builder.HasKey(nt => new { nt.NoticiaId, nt.TagId });

            builder.HasOne(nt => nt.Noticia)
                .WithMany(n => n.NoticiaTags)
                .HasForeignKey(nt => nt.NoticiaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(nt => nt.Tag)
                .WithMany(t => t.NoticiaTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(nt => new { nt.NoticiaId, nt.TagId })
            .HasDatabaseName("IX_NoticiaTags_NoticiaId_TagId");
        }
    }
}
