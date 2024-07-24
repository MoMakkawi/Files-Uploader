using Application.Helper;
using Application.Models.Attachments;

using AutoMapper;

using Domain.Entities;

namespace Application.Mappers;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentDTO>();

        CreateMap<Attachment, AttachmentWithBase64DTO>()
            .ForMember(
            dest => dest.Base64,
            opt => opt.MapFrom(src => AttachmentHelper.GetAsBase64(src.Path)));
    }
}
