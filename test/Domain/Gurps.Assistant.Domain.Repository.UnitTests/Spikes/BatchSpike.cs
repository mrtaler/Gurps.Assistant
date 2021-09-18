//using System;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.Spikes
//{
//    [TestFixture]
//    public class BatchSpike: TestBase
//    {
//        [Fact]
//        public void Repository_Should_BeginBatch()
//        {
//            var repository = new InMemRepository<Contact, Int32>();
            
//            using (var batch = repository.BeginBatch())
//            {
//                batch.Add(new Contact { Name = "Test User 1" });

//                var result = repository.GetAll();
//                result.Count().Should().Be(0); // shouldn't have really been added yet

//                batch.Add(new Contact { Name = "Test User 2" });

//                result = repository.GetAll();
//                result.Count().Should().Be(0); // shouldn't have really been added yet

//                batch.Commit();
//            }

//            repository.GetAll().Count().Should().Be(2);
//        }
//    }
//}
