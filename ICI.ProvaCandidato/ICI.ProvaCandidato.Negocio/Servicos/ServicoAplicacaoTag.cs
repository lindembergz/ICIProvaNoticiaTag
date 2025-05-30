using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Aplicacao.DTOs;
using ICiProvaCandidato.Aplicacao.Interfaces;
using ICiProvaCandidato.Dominio.Entidades;
using ICiProvaCandidato.Dominio.Repositorios;

namespace ICiProvaCandidato.Aplicacao.Servicos
{
    public class ServicoAplicacaoTag : IServicoAplicacaoTag
    {
        private readonly IRepositorioTag _repositorioTag;

        public ServicoAplicacaoTag(IRepositorioTag repositorioTag)
        {
            _repositorioTag = repositorioTag;
        }

        public async Task<TagDto?> ObterPorIdAsync(int id)
        {
            var tag = await _repositorioTag.ObterPorIdAsync(id);
            return tag != null ? ConverterParaDto(tag) : null;
        }

        public async Task<IEnumerable<TagDto>> ObterTodosAsync()
        {
            var tags = await _repositorioTag.ObterTodosAsync();
            return tags.Select(ConverterParaDto);
        }

        public async Task<IEnumerable<TagDto>> ObterPorNomeAsync(string nome)
        {
            var tags = await _repositorioTag.ObterPorNomeAsync(nome);
            return tags.Select(ConverterParaDto);
        }

        public async Task<TagDto> CriarAsync(TagDto tagDto)
        {
            // Validar se já existe tag com o mesmo nome
            if (await _repositorioTag.ExisteComNomeAsync(tagDto.Nome))
                throw new InvalidOperationException($"Já existe uma tag com o nome '{tagDto.Nome}'");

            var tag = new Tag(tagDto.Nome, tagDto.Descricao);
            var tagAdicionada = await _repositorioTag.AdicionarAsync(tag);

            return ConverterParaDto(tagAdicionada);
        }

        public async Task<TagDto> AtualizarAsync(TagDto tagDto)
        {
            var tag = await _repositorioTag.ObterPorIdAsync(tagDto.Id);
            if (tag == null)
                throw new ArgumentException("Tag não encontrada");

            // Validar se já existe outra tag com o mesmo nome
            if (await _repositorioTag.ExisteComNomeAsync(tagDto.Nome, tagDto.Id))
                throw new InvalidOperationException($"Já existe uma tag com o nome '{tagDto.Nome}'");

            tag.AtualizarNome(tagDto.Nome);
            tag.AtualizarDescricao(tagDto.Descricao);

            // Corrigido o problema de conversão de bool? para bool
            if (tagDto.Ativa.HasValue && tagDto.Ativa.Value)
                tag.Ativar();
            else
                tag.Desativar();

            await _repositorioTag.AtualizarAsync(tag);
            return ConverterParaDto(tag);
        }

        public async Task RemoverAsync(int id)
        {
            var tag = await _repositorioTag.ObterPorIdAsync(id);
            if (tag == null)
                throw new ArgumentException("Tag não encontrada");

            // Verificar se a tag está sendo usada em notícias
            if (tag.NoticiaTags.Any())
                throw new InvalidOperationException("Não é possível remover uma tag que está sendo utilizada em notícias");

            await _repositorioTag.RemoverAsync(id);
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _repositorioTag.ExisteAsync(id);
        }

        public async Task<bool> ExisteComNomeAsync(string nome, int? idExcluir = null)
        {
            return await _repositorioTag.ExisteComNomeAsync(nome, idExcluir);
        }

        private static TagDto ConverterParaDto(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Nome = tag.Nome,
                Descricao = tag.Descricao,
                Ativa = tag.Ativa
            };
        }
    }
}
