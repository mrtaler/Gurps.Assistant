using Gurps.Assistant.Domain.Repository.Aspects;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Serilog;
using Serilog.Events;

namespace Gurps.Assistant.Domain.Repository.Attributes
{
  public class RepositoryLoggingAttribute : RepositoryActionBaseAttribute
  {
    private readonly ILogger _logger;
    private LogEventLevel _logLevel = LogEventLevel.Debug;

    public RepositoryLoggingAttribute()
    {
      _logger = Serilog.Log.Logger;
      //_logger.csBeginScope("SharpRepository");
    }

    public LogEventLevel LogLevel
    {
      get => _logLevel;
      set => _logLevel = value;
    }

    private void Log(string message)
    {
      switch (_logLevel)
      {
        case LogEventLevel.Error:
          _logger.Error(message);
          break;
        case LogEventLevel.Information:
          _logger.Information(message);
          break;
        case LogEventLevel.Verbose:
          _logger.Verbose(message);
          break;
        default:
          _logger.Debug(message);
          break;
      }
    }

    public override void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context)
    {
      Log($"Initialized IRepository<{typeof(T).Name}, {typeof(TKey).Name}>");
    }

    public override bool OnAddExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
    {
      Log($"Adding {typeof(T).Name} entity");
      Log($"   {entity.ToString()}");
      return true;
    }

    public override void OnAddExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
    {
      Log($"Added {typeof(T).Name} entity");
      Log($"   {entity.ToString()}");
    }

    public override bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
    {
      Log($"Updating {typeof(T).Name} entity");
      Log($"   {entity.ToString()}");

      return true;
    }

    public override void OnUpdateExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
    {
      Log($"Updated {typeof(T).Name} entity");
      Log($"   {entity.ToString()}");
    }

    public override bool OnDeleteExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
    {
      Log($"Deleting {typeof(T).Name} entity");
      Log($"   {entity.ToString()}");

      return true;
    }

    public override void OnDeleteExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
    {
      Log($"Deleted {typeof(T).Name} entity");
      Log($"   {entity.ToString()}");
    }

    public override bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context)
    {
      Log($"Saving {typeof(T).Name} entity");

      return true;
    }

    public override void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context)
    {
      Log($"Saved {typeof(T).Name} entity");
    }

    public override bool OnGetExecuting<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executing Get: Id = {context.Id}");

      return true;
    }

    public override void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executed Get: Id = {context.Id}");
      Log(context.Repository.TraceInfo);
      Log($"{typeDisplay} Has Result: {context.HasResult} Cache Used: {context.Repository.CacheUsed}");
    }

    public override bool OnGetAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executing GetAll");

      return true;
    }

    public override void OnGetAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executed GetAll");
      Log(context.Repository.TraceInfo);
      Log($"{typeDisplay} Results: {context.NumberOfResults} Cache Used: {context.Repository.CacheUsed}");
    }

    public override bool OnFindExecuting<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executing Find: {context.Specification.Predicate}");

      return true;
    }

    public override void OnFindExecuted<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executed Find: {context.Specification.Predicate}");
      Log(context.Repository.TraceInfo);
      Log($"{typeDisplay} Results: {context.NumberOfResults} Cache Used: {context.Repository.CacheUsed}");
    }

    public override bool OnFindAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executing FindAll: {context.Specification.Predicate}");

      return true;
    }

    public override void OnFindAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
    {
      var typeDisplay = RepositoryTypeDisplay(context.Repository);

      Log($"{typeDisplay} Executed FindAll: {context.Specification.Predicate}");
      Log(context.Repository.TraceInfo);
      Log($"{typeDisplay} Results: {context.NumberOfResults} Cache Used: {context.Repository.CacheUsed}");
    }

    private static string RepositoryTypeDisplay<T, TKey>(IRepository<T, TKey> repository) where T : class
    {
      return $"[{repository.GetType().Name}<{typeof(T).Name},{typeof(TKey).Name}>]";
    }
  }
}
