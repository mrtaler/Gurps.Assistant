//using BoardGameAssistant.Domain.Repository.UnitTests.TestObjects;
//using BoardGameAssistant.Domain.Repository.UnitTests.TestObjects.Assert;
//using Xunit;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.Spikes
//{
 
//    public class LoggingSpikes : TestBase
//    {
//        [Fact]
//        public void Logging_Via_Aspects()
//        {
//            var repository = new InMemRepository<Contact, int>();


//            var contact1 = new Contact() {Name = "Contact 1"};
//            repository.Add(contact1);
//            repository.Add(new Contact() { Name = "Contact 2"});
//            repository.Add(new Contact() { Name = "Contact 3"});

//            contact1.Name += " EDITED";
//            repository.Update(contact1);

//            repository.Delete(2);

//            repository.FindAll(x => x.ContactId < 50);
//        }
//    }
//}
