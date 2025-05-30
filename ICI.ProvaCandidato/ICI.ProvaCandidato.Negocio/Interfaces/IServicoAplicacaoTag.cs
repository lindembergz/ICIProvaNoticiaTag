using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Aplicacao.DTOs;

namespace ICiProvaCandidato.Aplicacao.Interfaces
{
    public interface IServicoAplicacaoTag
    {
        Task<TagDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<TagDto>> ObterTodosAsync();
        Task<IEnumerable<TagDto>> ObterPorNomeAsync(string nome);
        Task<TagDto> CriarAsync(TagDto tagDto);
        Task<TagDto> AtualizarAsync(TagDto tagDto);
        Task RemoverAsync(int id);
        Task<bool> ExisteAsync(int id);
        Task<bool> ExisteComNomeAsync(string nome, int? idExcluir = null);
    }
}
