using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICiProvaCandidato.Dominio.Entidades
{
    public class Tag
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool? Ativa { get; private set; }

        private readonly List<NoticiaTag> _noticiaTags = new();
        public IReadOnlyCollection<NoticiaTag> NoticiaTags => _noticiaTags.AsReadOnly();

        protected Tag() { } 

        public Tag(string nome, string descricao = "")
        {
            ValidarNome(nome);
            Nome = nome;
            Descricao = descricao ?? string.Empty;
            Ativa = true;
        }

        public void AtualizarNome(string novoNome)
        {
            ValidarNome(novoNome);
            Nome = novoNome;
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            Descricao = novaDescricao ?? string.Empty;
        }

        public void Ativar() => Ativa = true;
        public void Desativar() => Ativa = false;

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da tag é obrigatório", nameof(nome));

            if (nome.Length > 50)
                throw new ArgumentException("Nome da tag não pode exceder 50 caracteres", nameof(nome));
        }
    }
}
