using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Database.Constants;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete
{
    public class OperationClaimMongoRepository : IOperationClaimDal
    {
        private IMongoCollection<OperationClaim> ConnectCollection()
        {
            var client = new MongoClient(DB.ConnectionString);
            var database = client.GetDatabase(DB.DbName);
            return database.GetCollection<OperationClaim>(DB.Collections.OperationClaimCollection);
        }

        public OperationClaim GetOperationClaimById(string id)
        {
            ObjectId docId = new ObjectId(id);
            return ConnectCollection().Find(x => x.Id == docId).FirstOrDefault();
        }

        public OperationClaim GetOperationClaimByName(string name)
        {
            return ConnectCollection().Find(x => x.Name == name).FirstOrDefault();
        }

        public List<OperationClaim> GetOperationClaims(Expression<Func<OperationClaim, bool>> expression = null)
        {
            return ConnectCollection().Find(expression).ToList();
        }
    }
}
