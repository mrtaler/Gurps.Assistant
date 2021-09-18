//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.PrimaryKey
//{
//    public class MongoDbPrimaryKeyTests
//    {
//       [Fact]
//        public void Should_Return_KeyInt1_Property()
//        {
//            // TO DEL
//            var _databaseName = MongoUrl.Create("mongodb://localhost/test").DatabaseName;
//            var cli           = new MongoClient("mongodb://localhost/test");

//            var repos    = new TestMongoDbRepository<ObjectKeys, int>();
//            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

//            propInfo.PropertyType.Should().Be(typeof(int));
//            propInfo.Name.Should().Be("KeyInt1");
//        }
//    }

//    internal class TestMongoDbRepository<T, TKey> : MongoDbRepository<T, TKey> where T : class, new()
//    {
//        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
//        {
//            return GetPrimaryKeyPropertyInfo();
//        }
//    }
//}
