using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Profiler
{
    /// <summary>
    /// Defines object-to-object mappings for authentication-related entities using AutoMapper.
    /// </summary>
    public class ResponseProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthProfiler"/> class and defines mappings
        /// between models and data transfer objects (DTOs).
        /// </summary>
        public ResponseProfile()
        {
            CreateMap<EmployeeResponseDto, UserLoginResponseDto>().ReverseMap();
            CreateMap<VendorResponseDto, UserLoginResponseDto>().ReverseMap();
            CreateMap<EmployeeResponse, EmployeesResponse>().ReverseMap();
            CreateMap<EmployeesResponse, EmployeeResponseDto>().ReverseMap();
            CreateMap<VendorResponse, VendorResponseDto>().ReverseMap();
            CreateMap<VendorDto, VendorResponse>().ReverseMap();
            CreateMap<Vendor, VendorResponse>().ReverseMap();
            CreateMap<VendorResponseDto, Vendor>().ReverseMap();
            CreateMap<Employee, EmployeeResponse>().ReverseMap();
            CreateMap<EmployeeResponse, EmployeeResponseDto>().ReverseMap();
            CreateMap<EmployeeResponseDto, Employee>().ReverseMap();
            CreateMap<Employee, EmployeeResponse>().ReverseMap();
            CreateMap<OrdersFoodItemDto, OrderFoodItem>().ReverseMap();
            CreateMap<Orders, OrdersDto>().ReverseMap();
            CreateMap<OrderFoodItem, OrderFoodItemDto>();
            CreateMap<InventoryItemDto, InventoryResponseDto>().ReverseMap();
            CreateMap<ReviewsDto, Reviews>().ReverseMap();
            CreateMap<ReviewsResponseDto, Reviews>().ReverseMap();
        }
    }
}
