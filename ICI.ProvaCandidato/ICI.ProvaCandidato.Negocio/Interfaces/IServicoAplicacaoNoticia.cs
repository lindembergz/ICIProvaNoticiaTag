using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Aplicacao.DTOs;

namespace ICiProvaCandidato.Aplicacao.Interfaces
{
    public interface IServicoAplicacaoNoticia
    {
        Task<NoticiaDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<NoticiaDto>> ObterTodosAsync();
        Task<IEnumerable<NoticiaDto>> ObterPorTagAsync(int tagId);
        Task<NoticiaDto> CriarAsync(NoticiaDto noticiaDto);
        Task<NoticiaDto> AtualizarAsync(NoticiaDto noticiaDto);
        Task RemoverAsync(int id);
        Task<bool> ExisteAsync(int id);

        Task<IEnumerable<NoticiaDto>> PesquisarAsync(string termo);
    }
}
