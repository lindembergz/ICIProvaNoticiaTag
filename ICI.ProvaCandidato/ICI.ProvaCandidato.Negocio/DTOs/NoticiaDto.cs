using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICiProvaCandidato.Aplicacao.DTOs
{
    public class NoticiaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título não pode exceder 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        public string Conteudo { get; set; } = string.Empty;

        public DateTime DataPublicacao { get; set; }
        public bool? Ativa { get; set; }

        public List<int> TagIds { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
    }
}
