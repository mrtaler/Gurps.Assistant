//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.PrimaryKey
//{
//    public class EfPrimaryKeyTests
//    {
//       [Fact]
//        public void Should_Return_KeyInt2_Property()
//        {
//            var repos    = new TestEfRepository<ObjectKeys, int>(new DbContext("test"));
//            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

//            propInfo.PropertyType.Should().Be(typeof(int));
//            propInfo.Name.Should().Be("KeyInt2");
//        }

//       [Fact]
//        public void Should_Return_KeyInt1_2_3_Property()
//        {
//            var repos    = new TestTripleKeyEfRepository<TripleObjectKeys, int, int, int>(new DbContext("test"));
//            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

//            propInfo[0].PropertyType.Should().Be(typeof(int));
//            propInfo[0].Name.Should().Be("KeyInt1");
//            propInfo[1].PropertyType.Should().Be(typeof(int));
//            propInfo[1].Name.Should().Be("KeyInt2");
//            propInfo[2].PropertyType.Should().Be(typeof(int));
//            propInfo[2].Name.Should().Be("KeyInt3");
//        }
//    }

//    internal class TestEfRepository<T, TKey> : EfRepository.EfRepository<T, TKey> where T : class, new()
//    {
//        public TestEfRepository(DbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null) : base(dbContext, cachingStrategy)
//        {
//        }

//        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
//        {
//            return GetPrimaryKeyPropertyInfo();
//        }
//    }

//    internal class TestTripleKeyEfRepository<T, TKey, TKey2, TKey3> : EfRepository.EfRepository<T, TKey, TKey2, TKey3> where T : class, new()
//    {
//        public TestTripleKeyEfRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null) : base(dbContext, cachingStrategy)
//        {
//        }

//        public PropertyInfo[] TestGetPrimaryKeyPropertyInfo()
//        {
//            return GetPrimaryKeyPropertyInfo();
//        }
//    }
//}
