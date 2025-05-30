using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICiProvaCandidato.Dominio.Entidades
{
    public class NoticiaTag
    {
        public int NoticiaId { get; private set; }
        public int TagId { get; private set; }

        public Noticia Noticia { get; private set; }
        public Tag Tag { get; private set; }

        protected NoticiaTag() { }

        public NoticiaTag(int noticiaId, int tagId)
        {
            NoticiaId = noticiaId;
            TagId = tagId;
        }
    }
}
