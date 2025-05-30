using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using ICiProvaCandidato.Aplicacao.Interfaces;
using ICiProvaCandidato.Web.Models;
using ICiProvaCandidato.Aplicacao.DTOs;
using System.Linq;
using ICiProvaCandidato.Web.Requests;

namespace ICIProvaCandidato.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly IServicoAplicacaoTag _servicoTag;

        public TagController(IServicoAplicacaoTag servicoTag)
        {
            _servicoTag = servicoTag;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _servicoTag.ObterTodosAsync();
            var viewModel = tags.Select(t => new TagViewModel
            {
                Id = t.Id,
                Nome = t.Nome,
                Descricao = t.Descricao,
                Ativa = t.Ativa ?? false // Explicitly handle the nullable bool
            });

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new TagViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { sucesso = false, erros });
            }

            try
            {
                var tagDto = new TagDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Ativa = true
                };

                await _servicoTag.CriarAsync(tagDto);
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
            var tag = await _servicoTag.ObterPorIdAsync(id);
            if (tag == null)
                return NotFound();

            var viewModel = new TagViewModel
            {
                Id = tag.Id,
                Nome = tag.Nome,
                Descricao = tag.Descricao,
                Ativa = tag.Ativa ?? false
            };

            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { sucesso = false, erros });
            }

            try
            {
                var tagDto = new TagDto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Ativa = model.Ativa
                };

                await _servicoTag.AtualizarAsync(tagDto);
                return Json(new { sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _servicoTag.ObterPorIdAsync(id);
            if (tag == null)
                return NotFound();

            var viewModel = new TagViewModel
            {
                Id = tag.Id,
                Nome = tag.Nome,
                Descricao = tag.Descricao,
                Ativa = tag.Ativa ?? false
            };

            return PartialView("_DeletePartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return Json(new { sucesso = false, mensagem = "ID inválido para exclusão" });
                }
                await _servicoTag.RemoverAsync(request.Id);
                return Json(new { sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }
    }
}