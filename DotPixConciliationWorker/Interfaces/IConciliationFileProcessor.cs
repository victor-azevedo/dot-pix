using DotPixConciliationWorker.Models.Dtos;

namespace DotPixConciliationWorker.Interfaces;

public interface IConciliationFileProcessor
{
    Task<OutPostConciliationDto> GetConciliationBalance(InConciliationQueueDto conciliationQueueDto);
}