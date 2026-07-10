using SpinaBets.Models;


namespace SpinaBets.Services.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetAll();
        Task<Game?> GetById(int id);

        Task Create(Game game);
        Task Update(Game game);
        Task Delete(int id);

        Task SetResult(int gameId, string result);
    }
}
