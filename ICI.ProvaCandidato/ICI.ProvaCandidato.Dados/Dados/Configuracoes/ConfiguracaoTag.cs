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
    public class ConfiguracaoTag : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .UseIdentityColumn();

            builder.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Descricao)
                .HasMaxLength(200);

            builder.Property(t => t.Ativa)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(t => t.Nome)
                .IsUnique()
                .HasDatabaseName("IX_Tags_Nome");
        }
    }
}
