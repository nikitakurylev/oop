using System;
using System.Collections.Generic;
using ReportsServer.Models;

namespace ReportsServer.Entities
{
    public interface IDatabase<T> where T : IModel
    {
        T Get(string uid);
        List<T> GetAll();
        List<T> GetAll(Predicate<T> match);
        void Update(T item);
        void Create(T item);
        void Remove(string uid);
        void Remove(T item);
    }
}