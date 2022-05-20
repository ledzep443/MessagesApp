using AutoMapper;
using DataAccess;
using Models;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatMessage, ChatMessageDTO>().ReverseMap();
        }
    }
}
