namespace DotPixConciliationWorker.Interfaces;

public interface IFileReader
{
    Task ReadBatchesAndProcess<T>(string filePath, int batchSize, Func<List<T>, Task> batchProcessor);
}