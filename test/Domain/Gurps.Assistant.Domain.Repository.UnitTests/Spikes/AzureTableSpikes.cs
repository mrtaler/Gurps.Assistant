//namespace BoardGameAssistant.Domain.Repository.UnitTests.Spikes
//{
//    [TestFixture]
//    public class AzureTableSpikes
//    {
//        [Fact]
//        public void TestAzureTableGet()
//        {
//            var repo = new AzureTableRepository<PoisonMessage, string, string>("UseDevelopmentStorage=true");
//            var item = repo.Get("QueueEmailModel", "3b80be6f-f28f-4768-99f1-6c181505ce86");

//            item.ShouldNotBeNull();
//        }
//    }

//    public class PoisonMessage : TableEntity
//    {
//        public string MessageType { get; set; }
//        public string Id { get; set; }
//        public int DequeueCount { get; set; }
//        public string Message { get; set; }
//    }
//}
