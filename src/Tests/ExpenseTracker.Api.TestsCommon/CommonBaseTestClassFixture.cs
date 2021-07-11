using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoMapper;
using ExpenseTracker.Core.Domain.AutoMapperProfiles;

namespace ExpenseTracker.Api.TestsCommon
{
    public abstract class CommonBaseTestClassFixture : IDisposable
    {
        protected static IMapper _mapper { get; }
       protected static IFixture _fixture { get; }

       static CommonBaseTestClassFixture()
       {
           _fixture = new Fixture();
           var config = new MapperConfiguration(cfg =>
           {
               cfg.AddMaps(typeof(ExpenseProfile).GetTypeInfo().Assembly);
           });

           _mapper = config.CreateMapper();
       }
        
        public void Dispose() { }
    }
}