//using System.Collections.Concurrent;
//using BoardGameHelper.Repository.SharpRepository.Interfaces;

//namespace BoardGameHelper.Repository.Test.TestObjects.Context
//{
//    public class MemoryDefaultDataContextFactory<TKey, T> :
//        IDataContextFactory<InMemoryDbContext<TKey, T>>
//    {
//        private readonly ConcurrentDictionary<TKey, T> _items;
//        private InMemoryDbContext<TKey, T> _dataContext;

//        public MemoryDefaultDataContextFactory(ConcurrentDictionary<TKey, T> items)
//        {
//            _items = items;
//        }

//        /// <inheritdoc />
//        public InMemoryDbContext<TKey, T> GetContext()
//        {
//            return _dataContext ??= new InMemoryDbContext<TKey, T>(_items);
//        }

//        /// <inheritdoc />
//        public void Dispose()
//        {
//            _dataContext?.Dispose();
//        }
//    }
//}
