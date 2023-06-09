﻿using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
