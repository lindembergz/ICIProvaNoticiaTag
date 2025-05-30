using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Entidades;
using ICiProvaCandidato.Infra.Dados.Configuracoes;
using Microsoft.EntityFrameworkCore;

namespace ICiProvaCandidato.Infra.Dados
{
    public class ContextoSistemaNoticiasDDD : DbContext
    {
        public ContextoSistemaNoticiasDDD(DbContextOptions<ContextoSistemaNoticiasDDD> options)
            : base(options)
        {
        }

        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoticiaTag> NoticiaTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfiguracaoNoticia());
            modelBuilder.ApplyConfiguration(new ConfiguracaoTag());
            modelBuilder.ApplyConfiguration(new ConfiguracaoNoticiaTag());

            base.OnModelCreating(modelBuilder);
        }
    }
}
