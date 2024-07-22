using System.Net.Mail;

using Application.Models.Attachments;

using AutoMapper;

namespace Application.Mappers;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentDTO>()
            .ReverseMap();
    }
}
