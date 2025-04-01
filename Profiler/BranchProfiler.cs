using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Profiler
{
    /// <summary>
    /// Branch profiler
    /// </summary>
    public class BranchProfiler : Profile
    {
        /// <summary>
        /// Branch profiler
        /// </summary>
        public BranchProfiler()
        {
            CreateMap<BranchDto, Branch>();
            CreateMap<Branch, BranchDto>();
        }
    }
}
