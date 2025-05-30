using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICiProvaCandidato.Dominio.Entidades
{
    public class Noticia
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Conteudo { get; private set; }
        public DateTime DataPublicacao { get; private set; }
        public bool? Ativa { get; private set; }

        private readonly List<NoticiaTag> _noticiaTags = new();
        public IReadOnlyCollection<NoticiaTag> NoticiaTags => _noticiaTags.AsReadOnly();

        protected Noticia() { } 

        public Noticia(string titulo, string conteudo)
        {
            ValidarTitulo(titulo);
            ValidarConteudo(conteudo);

            Titulo = titulo;
            Conteudo = conteudo;
            DataPublicacao = DateTime.UtcNow;
            Ativa = true;
        }
        public void AtualizarTitulo(string novoTitulo)
        {
            ValidarTitulo(novoTitulo);
            Titulo = novoTitulo;
        }

        public void AtualizarConteudo(string novoConteudo)
        {
            ValidarConteudo(novoConteudo);
            Conteudo = novoConteudo;
        }

        public void AdicionarTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (_noticiaTags.Any(nt => nt.TagId == tag.Id))
                return; // Tag já associada

            _noticiaTags.Add(new NoticiaTag(Id, tag.Id));
        }

        public void RemoverTag(int tagId)
        {
            var noticiaTag = _noticiaTags.FirstOrDefault(nt => nt.TagId == tagId);
            if (noticiaTag != null)
                _noticiaTags.Remove(noticiaTag);
        }

        public void Ativar() => Ativa = true;
        public void Desativar() => Ativa = false;

        private static void ValidarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título é obrigatório", nameof(titulo));

            if (titulo.Length > 200)
                throw new ArgumentException("Título não pode exceder 200 caracteres", nameof(titulo));
        }

        private static void ValidarConteudo(string conteudo)
        {
            if (string.IsNullOrWhiteSpace(conteudo))
                throw new ArgumentException("Conteúdo é obrigatório", nameof(conteudo));
        }
    }
}
