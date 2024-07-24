using Application.Models.Attachments;

using AutoMapper;

using Domain.Entities;

namespace Application.Mappers;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentDTO>()
            .ReverseMap();
    }
}
