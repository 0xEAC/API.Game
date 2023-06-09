﻿using API.Game.Backend.Database.Tables;

namespace API.Game.Backend.Interfaces
{
    public interface IAnimals
    {
        IEnumerable<Animal> GetAll();
        Animal Get(Guid id);
        string Feed(Guid id);
        string FeedAll();
        string Pet(Guid id);
        string Breed(Guid first, Guid second);
        string Generate();
        string Delete(Guid id);
    }
}
