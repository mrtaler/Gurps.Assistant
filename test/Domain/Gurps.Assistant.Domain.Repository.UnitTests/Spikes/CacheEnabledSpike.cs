//using System;
//using Microsoft.Extensions.Caching.Memory;

//namespace BoardGameAssistant.Domain.Repository.UnitTests.Spikes
//{
//	[TestFixture]
//	public class CacheEnabledSpike : TestBase
//	{
//        private ICachingProvider cacheProvider;

//        [SetUp]
//        public void Setup()
//        {
//            cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
//        }

//        [Fact]
//		public void CachingEnabled_Should_Be_False_With_NoCachingStrategy()
//		{
//			var repository = new InMemRepository<Contact, Int32>(new NoCachingStrategy<Contact, int>());
//			repository.CachingEnabled.Should().BeFalse();
//		}

//		[Fact]
//		public void CachingEnabled_Should_Be_True_With_TimeoutCachingStrategy()
//		{
//			var repository = new InMemRepository<Contact, Int32>(new TimeoutCachingStrategy<Contact, int>(60, cacheProvider));
//			repository.CachingEnabled.Should().BeTrue();
//		}

//		[Fact]
//		public void CachingEnabled_Should_Be_True_With_StandardCachingStrategy()
//		{
//			var repository = new InMemRepository<Contact, Int32>(new StandardCachingStrategy<Contact, int>(cacheProvider));
//			repository.CachingEnabled.Should().BeTrue();
//		}

//		[Fact]
//		public void CachingEnabled_Should_Be_True_When_CachingStrategy_Is_Changed_From_NoCachingStrategy()
//		{
//			var repository = new InMemRepository<Contact, Int32>(new NoCachingStrategy<Contact, int>());
//			repository.CachingEnabled.Should().BeFalse();
//			repository.CachingStrategy = new TimeoutCachingStrategy<Contact, int>(60, cacheProvider);
//			repository.CachingEnabled.Should().BeTrue();
//		}

//		[Fact]
//		public void CachingEnabled_Should_Be_False_When_CachingStrategy_Is_Changed_To_NoCachingStrategy()
//		{
//			var repository = new InMemRepository<Contact, Int32>(new TimeoutCachingStrategy<Contact, int>(60, cacheProvider));
//			repository.CachingEnabled.Should().BeTrue();
//			repository.CachingStrategy = new NoCachingStrategy<Contact, int>();
//			repository.CachingEnabled.Should().BeFalse();
//		}
//	}
//}
