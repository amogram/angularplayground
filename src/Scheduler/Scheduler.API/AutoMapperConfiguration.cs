using AutoMapper;
using Scheduler.API.ViewModels.Mappings;

namespace Scheduler.API
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
            });
        }
    }
}
