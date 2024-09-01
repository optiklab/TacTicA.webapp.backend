using System.Threading.Tasks;

namespace TacTicA.Common.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}