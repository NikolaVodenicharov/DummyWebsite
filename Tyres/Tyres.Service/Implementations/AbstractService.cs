using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data;

namespace Tyres.Service.Implementations
{
    public abstract class AbstractService
    {
        private protected TyresDbContext db;
        private protected IMapper mapper;

        public AbstractService(TyresDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
    }
}
