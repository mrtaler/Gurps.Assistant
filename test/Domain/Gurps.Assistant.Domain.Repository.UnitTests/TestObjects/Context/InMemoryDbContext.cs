//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using BoardGameHelper.Repository.InMemory;

//namespace BoardGameHelper.Repository.Test.TestObjects.Context
//{
//    public class InMemoryDbContext<TKey, T> : IMemoryDbContext<TKey, T>
//    {
//        public readonly ConcurrentDictionary<TKey, T> Items;

//        public InMemoryDbContext(ConcurrentDictionary<TKey, T> items)
//        {
//            Items = items;
//        }

//        /// <inheritdoc />
//        public void Dispose() => throw new System.NotImplementedException();

//        /// <exception cref = "NotImplementedException"> </exception>
//        /// <inheritdoc />
//        public int SaveChanges() => throw new System.NotImplementedException();

//        /// <inheritdoc />
//        public ConcurrentDictionary<TKey, T> GetItems()
//        {
//            return Items;
//        }
//    }
//}
