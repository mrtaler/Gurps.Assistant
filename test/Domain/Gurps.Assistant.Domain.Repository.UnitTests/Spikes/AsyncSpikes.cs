//using BoardGameAssistant.Domain.Repository.UnitTests.TestObjects;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.Spikes
//{

//    public class AsyncSpikes
//    {
//        private TestObjectContextCore context;

//        [SetUp]
//        public void Setup()
//        {
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
//                .UseSqlite(connection)
//                .Options;

//            // Run the test against one instance of the context
//            context = new TestObjectContextCore(options);
//            context.Database.EnsureCreated();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            context.Dispose();
//            context = null;
//        }

//        [Fact]
//        public void TestAsyncRequest()
//        {
//            var repo = new EfCoreRepository<Contact>(context);

//            var contacts = repo.GetAll().ToAsyncEnumerable().ToList();

//            contacts.Result.Count.Should().Be(0);
//        }
//    }
//}
