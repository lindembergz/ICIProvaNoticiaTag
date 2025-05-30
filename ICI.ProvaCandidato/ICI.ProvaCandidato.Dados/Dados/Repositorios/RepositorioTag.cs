using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Entidades;
using ICiProvaCandidato.Dominio.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace ICiProvaCandidato.Infra.Dados.Repositorios
{
    public class RepositorioTag : IRepositorioTag
    {
        private readonly ContextoSistemaNoticiasDDD _contexto;

        public RepositorioTag(ContextoSistemaNoticiasDDD contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        public async Task<Tag?> ObterPorIdAsync(int id)
        {
            return await _contexto.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tag>> ObterTodosAsync()
        {
            return await _contexto.Tags
                .AsNoTracking()
                .Where(t => t.Ativa == true)
                .OrderBy(t => t.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> ObterPorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Enumerable.Empty<Tag>();

            return await _contexto.Tags
                .AsNoTracking()
                .Where(t => t.Ativa == true && t.Nome.Contains(nome)) 
                .OrderBy(t => t.Nome)
                .ToListAsync();
        }

        public async Task<Tag> AdicionarAsync(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            await _contexto.Tags.AddAsync(tag);
            await _contexto.SaveChangesAsync();
            return tag;
        }

        public async Task AtualizarAsync(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _contexto.Tags.Update(tag);
            await _contexto.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var tag = await _contexto.Tags
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                throw new InvalidOperationException($"Tag com ID {id} não encontrada.");

            _contexto.Tags.Remove(tag);
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _contexto.Tags
                .AsNoTracking()
                .AnyAsync(t => t.Id == id);
        }

        public async Task<bool> ExisteComNomeAsync(string nome, int? idExcluir = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            var query = _contexto.Tags
                .AsNoTracking()
                .Where(t => t.Nome == nome);

            if (idExcluir.HasValue)
                query = query.Where(t => t.Id != idExcluir.Value);

            return await query.AnyAsync();
        }
    }
}
