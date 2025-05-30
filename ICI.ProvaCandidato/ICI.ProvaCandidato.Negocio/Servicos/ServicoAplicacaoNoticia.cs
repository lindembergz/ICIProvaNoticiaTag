using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICiProvaCandidato.Aplicacao.DTOs;
using ICiProvaCandidato.Aplicacao.Interfaces;
using ICiProvaCandidato.Dominio.Entidades;
using ICiProvaCandidato.Dominio.UoW;
using System.Collections.Generic;
using System.Collections;

namespace ICiProvaCandidato.Aplicacao.Servicos
{
    public class ServicoAplicacaoNoticia : IServicoAplicacaoNoticia
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicoAplicacaoNoticia(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NoticiaDto?> ObterPorIdAsync(int id)
        {
            var noticia = await _unitOfWork.RepositorioNoticia.ObterPorIdAsync(id);
            return noticia != null ? ConverterParaDto(noticia) : null;
        }

        public async Task<IEnumerable<NoticiaDto>> ObterTodosAsync()
        {
            var noticias = await _unitOfWork.RepositorioNoticia.ObterTodosAsync();
            return noticias.Select(ConverterParaDto);
        }

        public async Task<IEnumerable<NoticiaDto>> ObterPorTagAsync(int tagId)
        {
            var noticias = await _unitOfWork.RepositorioNoticia.ObterPorTagAsync(tagId);
            return noticias.Select(ConverterParaDto);
        }

        public async Task<NoticiaDto> CriarAsync(NoticiaDto noticiaDto)
        {
            var noticia = new Noticia(noticiaDto.Titulo, noticiaDto.Conteudo);
            var noticiaAdicionada = await _unitOfWork.RepositorioNoticia.AdicionarAsync(noticia);

            if (noticiaDto.TagIds?.Any() == true)
            {
                foreach (var tagId in noticiaDto.TagIds)
                {
                    var tag = await _unitOfWork.RepositorioTag.ObterPorIdAsync(tagId);
                    if (tag != null)
                        noticiaAdicionada.AdicionarTag(tag);
                }
                await _unitOfWork.RepositorioNoticia.AtualizarAsync(noticiaAdicionada);
                noticiaAdicionada = await _unitOfWork.RepositorioNoticia.ObterPorIdAsync(noticiaAdicionada.Id);
            }

            await _unitOfWork.CommitAsync();
            return ConverterParaDto(noticiaAdicionada);
        }

        public async Task<NoticiaDto> AtualizarAsync(NoticiaDto noticiaDto)
        {
            var noticia = await _unitOfWork.RepositorioNoticia.ObterPorIdAsync(noticiaDto.Id);
            if (noticia == null)
                throw new ArgumentException("Notícia não encontrada");

            noticia.AtualizarTitulo(noticiaDto.Titulo);
            noticia.AtualizarConteudo(noticiaDto.Conteudo);

            foreach (var noticiaTag in noticia.NoticiaTags.ToList())
            {
                noticia.RemoverTag(noticiaTag.TagId);
            }

            if (noticiaDto.TagIds?.Any() == true)
            {
                foreach (var tagId in noticiaDto.TagIds)
                {
                    var tag = await _unitOfWork.RepositorioTag.ObterPorIdAsync(tagId);
                    if (tag != null)
                        noticia.AdicionarTag(tag);
                }
            }

            await _unitOfWork.RepositorioNoticia.AtualizarAsync(noticia);
            await _unitOfWork.CommitAsync();

            noticia = await _unitOfWork.RepositorioNoticia.ObterPorIdAsync(noticiaDto.Id);
            return ConverterParaDto(noticia);
        }

        public async Task RemoverAsync(int id)
        {
            await _unitOfWork.RepositorioNoticia.RemoverAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _unitOfWork.RepositorioNoticia.ExisteAsync(id);
        }

        private static NoticiaDto ConverterParaDto(Noticia noticia)
        {
            return new NoticiaDto
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                DataPublicacao = noticia.DataPublicacao,
                Ativa = noticia.Ativa,
                Tags = noticia.NoticiaTags?.Select(nt => new TagDto
                {
                    Id = nt.Tag.Id,
                    Nome = nt.Tag.Nome,
                    Descricao = nt.Tag.Descricao,
                    Ativa = nt.Tag.Ativa
                }).ToList() ?? new List<TagDto>()
            };
        }

        public async  Task<IEnumerable<NoticiaDto>> PesquisarAsync(string termo)
        {
            var noticias = await _unitOfWork.RepositorioNoticia.PesquisarAsync(termo);

            return noticias.Select(ConverterParaDto);
        }
    }
}
