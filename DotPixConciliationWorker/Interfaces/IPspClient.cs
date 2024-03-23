using DotPixConciliationWorker.Models.Dtos;

namespace DotPixConciliationWorker.Interfaces;

public interface IPspClient
{
    Task PostConciliation(string pspApiConciliationUrl, OutPostConciliationDto conciliation);
}