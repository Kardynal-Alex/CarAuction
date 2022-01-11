using AutoMapper;
using System;
using System.Collections.Generic;

namespace Auction.BLL.Mapping
{
    public class AutoMapperHelper<A, B>
    {
        public B MapToType(A obj)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<A, B>());
            var mapper = new Mapper(config);
            var mappedList = mapper.Map<A, B>(obj);
            return mappedList;
        }
        public List<B> MapToTypeList(List<A> list)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<A, B>());
            var mapper = new Mapper(config);
            var mappedClass = mapper.Map<List<B>>(list);
            return mappedClass;
        }
    }
}
