using System.Text.Json;
using DotPixConciliationWorker.Interfaces;

namespace DotPixConciliationWorker.Services;

public class NdjsonReader : IFileReader
{
    public async Task ReadBatchesAndProcess<T>(string filePath, int batchSize, Func<List<T>, Task> batchProcessor)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("The specified file does not exist.", filePath);

        using var fileReader = new StreamReader(filePath);

        string? line;
        var batch = new List<T>(batchSize);
        while ((line = await fileReader.ReadLineAsync()) is not null)
        {
            var data = JsonSerializer.Deserialize<T>(line);
            if (data is not null)
            {
                batch.Add(data);
                if (batch.Count >= batchSize)
                {
                    await batchProcessor(batch);
                    batch.Clear();
                }
            }
        }

        if (batch.Count > 0)
        {
            await batchProcessor(batch);
        }
    }
}