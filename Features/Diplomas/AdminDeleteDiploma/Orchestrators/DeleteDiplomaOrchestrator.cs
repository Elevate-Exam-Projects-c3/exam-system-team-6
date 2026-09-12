using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators;

public record DeleteDiplomaOrchestrator(Guid DiplomaId) : IRequest<RequestResponse>;