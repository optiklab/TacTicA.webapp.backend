using System.Threading.Tasks;

namespace TacTicA.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}