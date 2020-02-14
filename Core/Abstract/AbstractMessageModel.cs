using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Abstract
{
    public abstract class AbstractMessageModel: AbstractMongoEntity
    {
        public virtual string Type { get; }
    }
}
