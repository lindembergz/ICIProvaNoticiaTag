using System.Threading.Tasks;
using ICiProvaCandidato.Dominio.Repositorios;

namespace ICiProvaCandidato.Dominio.UoW
{
    public interface IUnitOfWork
    {
        IRepositorioNoticia RepositorioNoticia { get; }
        IRepositorioTag RepositorioTag { get; }
        Task CommitAsync();
    }
}

