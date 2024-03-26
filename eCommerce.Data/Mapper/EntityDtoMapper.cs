using AutoMapper;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Mapper
    {
    public class EntityDtoMapper:Profile
        {
        public EntityDtoMapper()
        {
            CreateMap<Product, AddProductDTO>();
            CreateMap<Product, ProductInfoDTO>()
            .ForMember(dto => dto.BrandName, conf => conf.MapFrom(p => p.Brand.BrandName))
            .ForMember(dto => dto.CategoryName, conf => conf.MapFrom(p => p.Category.CategoryName));
            CreateMap<AddProductDTO, Product>();

        }
    }
    }
