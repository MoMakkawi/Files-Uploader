using Application.Contracts;

using Domain.Entities;

using Persistence.Data;
using Persistence.Services.Repositories;

namespace Persistence.Repositories;

public class AttachmentRepositoryAsync(MySQLDBContext context) 
    : GenericRepositoryAsync<Attachment>(context), IAttachmentRepositoryAsync;
