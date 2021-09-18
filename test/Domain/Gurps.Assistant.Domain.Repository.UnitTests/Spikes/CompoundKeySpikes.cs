//using BoardGameAssistant.Domain.Repository.UnitTests.TestObjects;
//using Xunit;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.Spikes
//{
//    public class CompoundKeySpikes
//    {
//        [Fact]
//        public void CompoundKeyRepository_Should_Work()
//        {
//            var repository = new InMemRepository<CompoundKeyItemInts, int, int>();

//            repository.Add(new CompoundKeyItemInts { SomeId = 1, AnotherId = 1, Title = "1-1" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 1, AnotherId = 2, Title = "1-2" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 1, AnotherId = 3, Title = "1-3" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 2, AnotherId = 1, Title = "2-1" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 2, AnotherId = 2, Title = "2-2" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 2, AnotherId = 3, Title = "2-3" });

//            repository.Get(1, 1).Title.Should().Be("1-1");
//            repository.Get(2, 1).Title.Should().Be("2-1");
//            repository.Get(1, 2).Title.Should().Be("1-2");

//            repository.FindAll(x => x.SomeId == 1).Count().Should().Be(3);
//        }

//        [Fact]
//        public void CompoundKeyRepositoryNoGenerics_Should_Work()
//        {
//            var repository = new InMemoryCompoundKeyRepository<CompoundKeyItemInts>();

//            repository.Add(new CompoundKeyItemInts { SomeId = 1, AnotherId = 1, Title = "1-1" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 1, AnotherId = 2, Title = "1-2" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 1, AnotherId = 3, Title = "1-3" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 2, AnotherId = 1, Title = "2-1" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 2, AnotherId = 2, Title = "2-2" });
//            repository.Add(new CompoundKeyItemInts { SomeId = 2, AnotherId = 3, Title = "2-3" });

//            repository.Get(1, 1).Title.Should().Be("1-1");
//            repository.Get(2, 1).Title.Should().Be("2-1");
//            repository.Get(1, 2).Title.Should().Be("1-2");

//            repository.FindAll(x => x.SomeId == 1).Count().Should().Be(3);
//        }

//        [Fact]
//        public void TripleCompoundKeyRepository_Should_Work()
//        {
//            var repository = new InMemRepository<TripleCompoundKeyItemInts, int, int, int>();

//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 1, AnotherId = 1, LastId = 10, Title = "1-1-10" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 1, AnotherId = 2, LastId = 11, Title = "1-2-11" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 1, AnotherId = 3, LastId = 10, Title = "1-3-10" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 2, AnotherId = 1, LastId = 11, Title = "2-1-11" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 2, AnotherId = 2, LastId = 10, Title = "2-2-10" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 2, AnotherId = 3, LastId = 11, Title = "2-3-11" });

//            repository.Get(1, 1, 10).Title.Should().Be("1-1-10");
//            repository.Get(2, 1, 11).Title.Should().Be("2-1-11");
//            repository.Get(1, 2, 11).Title.Should().Be("1-2-11");

//            repository.FindAll(x => x.LastId == 11).Count().Should().Be(3);
//        }

//        [Fact]
//        public void TripleCompoundKeyRepositoryNoGenerics_Should_Work()
//        {
//            var repository = new InMemoryCompoundKeyRepository<TripleCompoundKeyItemInts>();

//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 1, AnotherId = 1, LastId = 10, Title = "1-1-10" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 1, AnotherId = 2, LastId = 11, Title = "1-2-11" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 1, AnotherId = 3, LastId = 10, Title = "1-3-10" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 2, AnotherId = 1, LastId = 11, Title = "2-1-11" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 2, AnotherId = 2, LastId = 10, Title = "2-2-10" });
//            repository.Add(new TripleCompoundKeyItemInts { SomeId = 2, AnotherId = 3, LastId = 11, Title = "2-3-11" });

//            repository.Get(1, 1, 10).Title.Should().Be("1-1-10");
//            repository.Get(2, 1, 11).Title.Should().Be("2-1-11");
//            repository.Get(1, 2, 11).Title.Should().Be("1-2-11");

//            repository.FindAll(x => x.LastId == 11).Count().Should().Be(3);
//        }
//    }
//}
