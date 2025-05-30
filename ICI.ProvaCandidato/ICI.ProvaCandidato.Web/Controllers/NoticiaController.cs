using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using ICiProvaCandidato.Aplicacao.Interfaces;
using ICiProvaCandidato.Web.Models;
using ICiProvaCandidato.Aplicacao.DTOs;
using System.Linq;
using ICiProvaCandidato.Web.Requests;
using System.Collections.Generic;

namespace ICiProvaCandidato.Web.Controllers
{
    public class NoticiaController : Controller
    {
        private readonly IServicoAplicacaoNoticia _servicoNoticia;
        private readonly IServicoAplicacaoTag _servicoTag;

        public NoticiaController(
            IServicoAplicacaoNoticia servicoNoticia,
            IServicoAplicacaoTag servicoTag)
        {
            _servicoNoticia = servicoNoticia;
            _servicoTag = servicoTag;
        }

        public async Task<IActionResult> Index(string termoPesquisa = "")
        {
            IEnumerable<NoticiaDto> noticias;
            if (string.IsNullOrWhiteSpace(termoPesquisa))
            {
                noticias = await _servicoNoticia.ObterTodosAsync();
            }
            else
            {
                noticias = await _servicoNoticia.PesquisarAsync(termoPesquisa);
            }

            var viewModel = noticias.Select(n => new NoticiaViewModel
            {
                Id = n.Id,
                Titulo = n.Titulo,
                Conteudo = n.Conteudo,
                DataPublicacao = n.DataPublicacao,
                Ativa = n.Ativa,
                Tags = n.Tags.Select(t => new TagViewModel
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Descricao = t.Descricao
                }).ToList()
            });

            ViewBag.TermoPesquisa = termoPesquisa;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _servicoTag.ObterTodosAsync();
            ViewBag.Tags = tags;
            return PartialView("_CreatePartial", new NoticiaViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] NoticiaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { sucesso = false, erros });
            }


            try
            {
                var noticiaDto = new NoticiaDto
                {
                    Titulo = model.Titulo,
                    Conteudo = model.Conteudo,
                    TagIds = model.TagIdsSelecionadas
                };

                await _servicoNoticia.CriarAsync(noticiaDto);
                return Json(new { sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var noticia = await _servicoNoticia.ObterPorIdAsync(id);
            if (noticia == null)
                return NotFound();

            var tags = await _servicoTag.ObterTodosAsync();
            ViewBag.Tags = tags;

            var viewModel = new NoticiaViewModel
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                TagIdsSelecionadas = noticia.Tags.Select(t => t.Id).ToList()
            };

            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] NoticiaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { sucesso = false, erros });
            }


            try
            {
                var noticiaDto = new NoticiaDto
                {
                    Id = model.Id,
                    Titulo = model.Titulo,
                    Conteudo = model.Conteudo,
                    TagIds = model.TagIdsSelecionadas
                };

                await _servicoNoticia.AtualizarAsync(noticiaDto);
                return Json(new { sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var noticia = await _servicoNoticia.ObterPorIdAsync(id);
            if (noticia == null)
                return NotFound();

            var viewModel = new NoticiaViewModel
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                DataPublicacao = noticia.DataPublicacao
            };

            return PartialView("_DeletePartial", viewModel);
        }

        /*
         A razão para usar [HttpPost] em vez de [HttpDelete] para operações de exclusão em ASP.NET Core MVC 
         está relacionada às limitações dos formulários HTML e questões de segurança.
         */
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return Json(new { sucesso = false, mensagem = "ID inválido para exclusão" });
                }
                await _servicoNoticia.RemoverAsync(request.Id);
                return Json(new { sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }


    }
}
