using Core.Abstract;
using DataAccess.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete
{
    public class BaseMongoRepository<TEntity> : IBaseRespository<TEntity> where TEntity : AbstractMongoEntity, new()
    {
        private readonly IMongoCollection<TEntity> mongoCollection;

        public BaseMongoRepository(string mongoDBConnectionString, string dbName, string collectionName)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            var client = new MongoClient(mongoDBConnectionString);
            var database = client.GetDatabase(dbName);
            mongoCollection = database.GetCollection<TEntity>(collectionName);
        }

        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> expr = null)
        {
            if (expr != null)
                return mongoCollection.Find(expr).ToList();

            return mongoCollection.Find(entity => true).ToList();
        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> expr)
        {
            return mongoCollection.Find(expr).FirstOrDefault();
        }

        public virtual TEntity GetById(string id)
        {
            return mongoCollection.Find(x => x.Id == id).FirstOrDefault();
        }

        public virtual void Create(TEntity entity)
        {
            mongoCollection.InsertOne(entity);
        }
        public virtual void Update(string id, TEntity entity)
        {
            mongoCollection.ReplaceOne(x => x.Id == id, entity, new ReplaceOptions { IsUpsert = false});
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity != null)
            {
                mongoCollection.DeleteOne(x => x.Id == entity.Id);
            }
        }

        public virtual void Delete(string id)
        {
            mongoCollection.DeleteOne(x => x.Id == id);
        }
    }
}
