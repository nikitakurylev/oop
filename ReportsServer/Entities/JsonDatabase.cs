using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ReportsServer.Models;

namespace ReportsServer.Entities
{
    public class JsonDatabase<T> : IDatabase<T> where T : IModel
    {
        private readonly string _path;

        public JsonDatabase(string path)
        {
            _path = path;
        }

        public T Get(string uid)
        {
            if (File.Exists(GetFilename(uid)))
                return JsonSerializer.Deserialize<T>(File.ReadAllText(GetFilename(uid)));
            return default;
        }

        public List<T> GetAll()
        {
            var result = new List<T>();
            foreach (string filename in Directory.GetFiles(_path))
               result.Add(JsonSerializer.Deserialize<T>(File.ReadAllText(filename))); 

            return result;
        }

        public List<T> GetAll(Predicate<T> match)
        {
            return GetAll().FindAll(match);
        }

        public void Update(T item)
        {
            Create(item);
        }

        public void Create(T item)
        {
            File.WriteAllText(GetFilename(item.Uid), JsonSerializer.Serialize<T>(item));
        }

        public void Remove(string uid)
        {
            if (File.Exists(GetFilename(uid)))
                File.Delete(GetFilename(uid));
        }

        public void Remove(T item)
        {
            Remove(item.Uid);
        }

        private string GetFilename(string uid)
        {
            return _path + uid + ".json";
        }
    }
}