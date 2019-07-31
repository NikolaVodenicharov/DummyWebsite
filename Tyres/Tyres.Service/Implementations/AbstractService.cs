using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data;

namespace Tyres.Service.Implementations
{
    public abstract class AbstractService
    {
        protected TyresDbContext db;

        public AbstractService(TyresDbContext db)
        {
            this.db = db;
        }
    }
}
