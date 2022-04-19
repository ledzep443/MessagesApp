using AutoMapper;
using DataAccess;
using Models;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Chat, ChatDTO>().ReverseMap();
        }
    }
}
