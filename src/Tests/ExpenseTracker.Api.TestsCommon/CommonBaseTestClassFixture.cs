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
        protected static IMapper Mapper { get; }
       protected static IFixture Fixture { get; }

       static CommonBaseTestClassFixture()
       {
           Fixture = new Fixture();
           var config = new MapperConfiguration(cfg =>
           {
               cfg.AddMaps(typeof(ExpenseProfile).GetTypeInfo().Assembly);
           });

           Mapper = config.CreateMapper();
       }
        
        public void Dispose() { }
    }
}