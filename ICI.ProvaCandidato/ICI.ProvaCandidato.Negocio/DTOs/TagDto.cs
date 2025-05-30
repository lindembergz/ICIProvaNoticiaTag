using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICiProvaCandidato.Aplicacao.DTOs
{
    public class TagDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome não pode exceder 50 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Descrição não pode exceder 200 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        public bool? Ativa { get; set; }
    }
}
