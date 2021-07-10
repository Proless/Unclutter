using System.Collections.Generic;
using Unclutter.SDK.Data;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Games
{
    public interface IGamesProvider : IDataRepository<IGameDetails, long>
    {
        IEnumerable<IGameDetails> EnumerateGames();
        void Save(IEnumerable<IGameDetails> games);
    }
}
