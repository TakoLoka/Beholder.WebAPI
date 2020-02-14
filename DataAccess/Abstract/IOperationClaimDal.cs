using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IOperationClaimDal
    {
        List<OperationClaim> GetOperationClaims(Expression<Func<OperationClaim, bool>> expr);
        OperationClaim GetOperationClaimById(string id);
        OperationClaim GetOperationClaimByName(string name);
    }
}
