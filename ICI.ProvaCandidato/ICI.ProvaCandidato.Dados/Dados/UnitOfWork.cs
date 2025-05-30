using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Repositorios;
using ICiProvaCandidato.Infra.Dados;
using ICiProvaCandidato.Infra.Dados.Repositorios;

namespace ICiProvaCandidato.Dominio.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextoSistemaNoticiasDDD _contexto;

        public IRepositorioNoticia RepositorioNoticia { get; }
        public IRepositorioTag RepositorioTag { get; }

        public UnitOfWork(ContextoSistemaNoticiasDDD contexto)
        {
            _contexto = contexto;
            RepositorioNoticia = new RepositorioNoticia(_contexto);
            RepositorioTag = new RepositorioTag(_contexto);
        }

        public async Task CommitAsync()
        {
            await _contexto.SaveChangesAsync();
        }
    }
}
