using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Entidades;
using ICiProvaCandidato.Dominio.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace ICiProvaCandidato.Infra.Dados.Repositorios
{
    public class RepositorioNoticia : IRepositorioNoticia
    {
        private readonly ContextoSistemaNoticiasDDD _contexto;

        public RepositorioNoticia(ContextoSistemaNoticiasDDD contexto)
        {
            _contexto = contexto;
        }

        private static readonly Func<ContextoSistemaNoticiasDDD, int, Task<Noticia?>> _getByIdQuery =
      EF.CompileAsyncQuery((ContextoSistemaNoticiasDDD context, int id) =>
          context.Noticias
              .Include(n => n.NoticiaTags)
              .ThenInclude(nt => nt.Tag)
              .AsNoTracking()
              .FirstOrDefault(n => n.Id == id));

        public async Task<Noticia?> ObterPorIdAsync(int id)
        {
            return await _getByIdQuery(_contexto, id);

            /*return await _contexto.Noticias
                .Include(n => n.NoticiaTags)
                .ThenInclude(nt => nt.Tag)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);*/
        }

        public async Task<IEnumerable<Noticia>> ObterTodosAsync()
        {
            return await _contexto.Noticias
                .Include(n => n.NoticiaTags)
                .ThenInclude(nt => nt.Tag)
                .AsNoTracking()
                .OrderByDescending(n => n.DataPublicacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Noticia>> ObterPorTagAsync(int tagId)
        {
            return await _contexto.Noticias
                .Include(n => n.NoticiaTags)
                .ThenInclude(nt => nt.Tag)
                .Where(n => n.NoticiaTags.Any(nt => nt.TagId == tagId))
                .AsNoTracking()
                .OrderByDescending(n => n.DataPublicacao)
                .ToListAsync();
        }

        public async Task<Noticia> AdicionarAsync(Noticia noticia)
        {
            _contexto.Noticias.Add(noticia);
            await _contexto.SaveChangesAsync();
            return noticia;
        }

        public async Task AtualizarAsync(Noticia noticia)
        {
            _contexto.Entry(noticia).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var noticia = await _contexto.Noticias.FindAsync(id);
            if (noticia != null)
            {
                _contexto.Noticias.Remove(noticia);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _contexto.Noticias.AnyAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Noticia>> PesquisarAsync(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return await ObterTodosAsync();

            termo = termo.ToLower();
            return await _contexto.Noticias
                .Include(n => n.NoticiaTags)
                .ThenInclude(nt => nt.Tag)
                .AsNoTracking()
                .Where(n => n.Titulo.ToLower().Contains(termo) || n.Conteudo.ToLower().Contains(termo))
                .OrderByDescending(n => n.DataPublicacao)
                .ToListAsync();
        }
    }
}
