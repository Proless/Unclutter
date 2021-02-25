using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Unclutter.NexusAPI.DataModels;
using Unclutter.SDK.IModels;
using Unclutter.Services.Games;

namespace Unclutter.Services.ModelMapper
{
    public class ModelMapper : IModelMapper
    {
        private readonly IMapper _mapper;

        public ModelMapper()
        {
            _mapper = new MapperConfiguration
            (
                cfg =>
                {
                    cfg.AllowNullCollections = true;
                    cfg.CreateMap<NexusGameCategory, GameCategory>();
                    cfg.CreateMap<NexusGame, GameDetails>().
                        ForMember(g => g.Categories, opt => opt.MapFrom<IEnumerable<IGameCategory>>((gSource, gDest, mDest) =>
                        {
                            return gSource.Categories.Select(gameCategory => new GameCategory { GameId = gSource.Id, Id = gameCategory.Id, Name = gameCategory.Name, ParentCategoryId = gameCategory.ParentCategoryId });
                        }));
                }
            ).CreateMapper();
        }

        /// <summary>
        /// Try to map from a source object to destination object.
        /// </summary>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="obj">The source object to map from.</param>
        /// <returns>The mapped destination object or a default value.</returns>
        public TDestination MapOrDefault<TDestination>(object obj)
        {
            try
            {
                return !Equals(obj, default(TDestination)) ? _mapper.Map<TDestination>(obj) : default;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
