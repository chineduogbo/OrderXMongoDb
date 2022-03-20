using AutoMapper;
using OrderXMongoWebApi.Models;
using OrderXMongoWebApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CreateEventDto, Events>();
            CreateMap<Events, EventDto>();
            CreateMap<Events, StockDto>();
            CreateMap<Order, CreateOrderDto>();
            CreateMap< CreateOrderDto, Order>();
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>();
        }
    }
}
