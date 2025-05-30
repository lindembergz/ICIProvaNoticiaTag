using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Entidades;

namespace ICiProvaCandidato.Dominio.Repositorios
{
    public interface IRepositorioTag
    {
        Task<Tag?> ObterPorIdAsync(int id);
        Task<IEnumerable<Tag>> ObterTodosAsync();
        Task<IEnumerable<Tag>> ObterPorNomeAsync(string nome);
        Task<Tag> AdicionarAsync(Tag tag);
        Task AtualizarAsync(Tag tag);
        Task RemoverAsync(int id);
        Task<bool> ExisteAsync(int id);
        Task<bool> ExisteComNomeAsync(string nome, int? idExcluir = null);
    }
}
