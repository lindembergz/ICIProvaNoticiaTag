using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ICiProvaCandidato.Web.Models
{
    public class NoticiaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título não pode exceder 200 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; } = string.Empty;

        [Display(Name = "Data de Publicação")]
        public DateTime DataPublicacao { get; set; }

        public bool? Ativa { get; set; }

        public List<TagViewModel> Tags { get; set; } = new();
        public List<int> TagIdsSelecionadas { get; set; } = new();
    }
}
