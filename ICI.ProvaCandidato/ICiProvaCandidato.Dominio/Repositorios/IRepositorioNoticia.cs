using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Entidades;

namespace ICiProvaCandidato.Dominio.Repositorios
{
    public interface IRepositorioNoticia
    {
        Task<Noticia?> ObterPorIdAsync(int id);
        Task<IEnumerable<Noticia>> ObterTodosAsync();
        Task<IEnumerable<Noticia>> ObterPorTagAsync(int tagId);
        Task<Noticia> AdicionarAsync(Noticia noticia);
        Task AtualizarAsync(Noticia noticia);
        Task RemoverAsync(int id);
        Task<bool> ExisteAsync(int id);

        Task<IEnumerable<Noticia>> PesquisarAsync(string termo);
    }
}
