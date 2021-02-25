using System.Collections.Generic;
using Unclutter.SDK.IModels;
using Unclutter.Services.Data;

namespace Unclutter.Services.Games
{
    public interface IGamesProvider : IDataRepository<IGameDetails, long>
    {
        IEnumerable<IGameDetails> EnumerateGames();
        void Save(IEnumerable<IGameDetails> games);
    }
}
