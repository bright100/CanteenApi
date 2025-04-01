using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Profiler
{
    /// <summary>
    /// Defines object-to-object mappings for authentication-related entities using AutoMapper.
    /// </summary>
    public class AuthProfiler : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthProfiler"/> class and defines mappings
        /// between authentication-related models and data transfer objects (DTOs).
        /// </summary>
        public AuthProfiler()
        {
            // Mapping Vendor to VendorDto and vice versa
            CreateMap<Vendor, VendorDto>();
            CreateMap<VendorDto, Vendor>();

            // Mapping EmployeesResponse to Employee and vice versa
            CreateMap<EmployeesResponse, Employee>();
            CreateMap<Employee, EmployeesResponse>();

            // Mapping TokenModel to UserLoginResponseDto and vice versa
            CreateMap<TokenModel, UserLoginResponseDto>();
            CreateMap<UserLoginResponseDto, TokenModel>();
        }
    }

}
