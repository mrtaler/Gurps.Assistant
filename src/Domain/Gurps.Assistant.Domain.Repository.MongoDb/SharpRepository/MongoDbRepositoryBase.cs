﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.FetchStrategies;
using Gurps.Assistant.Domain.Repository.Helpers;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace Gurps.Assistant.Domain.Repository.MongoDb.SharpRepository
{
  public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey>
      where T : class, new()
  {
    private readonly IMongoCollection<T> collection;
    static readonly object _lock = new();

    private readonly Dictionary<Type, BsonType> _keyTypeToBsonType =
        new()
        {
          { typeof(string), BsonType.String },
          { typeof(Guid), BsonType.ObjectId },
          { typeof(ObjectId), BsonType.ObjectId },
          { typeof(byte[]), BsonType.ObjectId }
        };

    private readonly Dictionary<Type, IIdGenerator> _keyTypeToBsonGenerator =
        new()
        {
          { typeof(string), new StringObjectIdGenerator() },
          { typeof(Guid), new GuidGenerator() },
          { typeof(ObjectId), new ObjectIdGenerator() },
          { typeof(byte[]), new BsonBinaryDataGuidGenerator(GuidRepresentation.Standard) }
        };

    internal MongoDbRepositoryBase(IMongoCollection<T> collection, ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(cachingStrategy)
    {
      this.collection = collection;
      Initialize();
    }

    private void Initialize(IMongoDatabase mongoDatabase = null)
    {

      if (BsonClassMap.IsClassMapRegistered(typeof(T)))
        return;

      lock (_lock)
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
          var primaryKeyPropInfo = GetPrimaryKeyPropertyInfo();

          BsonClassMap.RegisterClassMap<T>(cm =>
          {
            cm.AutoMap();
            if (cm.IdMemberMap == null)
            {
              cm.SetIdMember(new BsonMemberMap(cm, primaryKeyPropInfo));

              if (_keyTypeToBsonType.ContainsKey(typeof(TKey)) && _keyTypeToBsonGenerator.ContainsKey(typeof(TKey)))
              {
                cm.IdMemberMap.SetSerializer(new StringSerializer(_keyTypeToBsonType[typeof(TKey)]));
                cm.IdMemberMap.SetIdGenerator(_keyTypeToBsonGenerator[typeof(TKey)]);
              }
            }

            cm.Freeze();
          }
          );
        }
    }

    private IMongoCollection<T> BaseCollection()
    {
      return collection;
    }

    protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
    {
      return BaseCollection().AsQueryable();
    }

    protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
    {
      var keyBsonType = ((StringSerializer)BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.GetSerializer()).Representation;
      var keyMemberName = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.MemberName;
      if (IsValidKey(key))
      {
        var keyBsonValue = BsonTypeMapper.MapToBsonValue(key, keyBsonType);
        var filter = Builders<T>.Filter.Eq(keyMemberName, keyBsonValue);
        return BaseCollection().Find(filter).FirstOrDefault();
      }
      else return default;
    }

    #region Math
    public override int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    #endregion

    protected override void AddItem(T entity)
    {
      BaseCollection().InsertOne(entity);
    }

    protected override void DeleteItem(T entity)
    {
      GetPrimaryKey(entity, out var pkValue);

      if (IsValidKey(pkValue))
      {
        var pkName = GetPrimaryKeyPropertyInfo().Name;
        var filter = Builders<T>.Filter.Eq(pkName, pkValue);
        BaseCollection().DeleteOne(filter);
      }
    }

    protected override void UpdateItem(T entity)
    {
      GetPrimaryKey(entity, out var pkValue);
      if (IsValidKey(pkValue))
      {
        var pkName = GetPrimaryKeyPropertyInfo().Name;
        var filter = Builders<T>.Filter.Eq(pkName, pkValue);
        BaseCollection().ReplaceOne(filter, entity);
      }
    }

    protected override PropertyInfo GetPrimaryKeyPropertyInfo()
    {
      // checks for the MongoDb BsonIdAttribute and if not there no the normal checks
      var type = typeof(T);
      var keyType = typeof(TKey);

      return type.GetProperties().FirstOrDefault(x => x.HasAttribute<BsonIdAttribute>() && x.PropertyType == keyType)
          ?? base.GetPrimaryKeyPropertyInfo();
    }

    public override bool GenerateKeyOnAdd
    {
      get { return base.GenerateKeyOnAdd; }
      set
      {
        if (value == false)
        {
          throw new NotSupportedException("Mongo DB driver always generates key values. SharpRepository can't avoid it.");
        }

        base.GenerateKeyOnAdd = value;
      }
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public override void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private static bool IsValidKey(TKey key)
    {
      return !string.IsNullOrEmpty(key.ToString());
    }
  }
}