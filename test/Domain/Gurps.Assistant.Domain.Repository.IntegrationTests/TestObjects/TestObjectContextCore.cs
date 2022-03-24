using System;
using System.Collections.Generic;
using Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  public class TestObjectContextCore : DbContext, ICoreDbContext
  {
    public ICollection<string> QueryLog;

    public TestObjectContextCore(DbContextOptions<TestObjectContextCore> options)
        : base(options)
    {
      QueryLog = new List<string>();
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<EmailAddress> EmailAddresses { get; set; }

    // set the Compound Key for the User object
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().HasKey(u => new { u.Username, u.Age });
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var lf = new LoggerFactory();
      lf.AddProvider(new TestLoggerProvider(ref QueryLog));
      optionsBuilder.UseLoggerFactory(lf);
    }
  }

  internal class TestLoggerProvider : ILoggerProvider
  {
    public ICollection<string> QueryLog;

    public TestLoggerProvider(ref ICollection<string> queryLog)
    {
      QueryLog = queryLog;
    }

    public ILogger CreateLogger(string categoryName)
    {
      return new TestLogger(ref QueryLog);
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }

  internal class TestLogger : ILogger
  {
    public TestLogger(ref ICollection<string> queryLog)
    {
      QueryLog = queryLog;
    }

    public ICollection<string> QueryLog { get; private set; }

    public IDisposable BeginScope<TState>(TState state)
    {
      return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      QueryLog.Add(formatter(state, exception));
    }
  }
}
