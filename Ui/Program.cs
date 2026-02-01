using Core.Services;
using Ui.Services.Reader.Implementations;
using Ui.Services.Writer.Implementations;

namespace Ui;

internal class Program
{
    public static async Task Main()
    {
        var reader = new ConsoleReader();
        var writer = new ConsoleWriter();

        writer.WriteLine("Please enter ip file path:");
        var filePath = reader.ReadLine();

        var ipDataService = new IpDataCoreService(filePath, writer);
        var result = await ipDataService.GetIpProcessingResult();

        if (result is not null)
        {
            writer.WriteLine($"{Environment.NewLine}=== RESULTS ==={Environment.NewLine}");

            foreach (var count in result.IpCountByCountry)
            {
                writer.WriteLine($"{count.Key} - {count.Value};");
            }

            writer.WriteLine($"{Environment.NewLine}Most popular country is: {result.MostPopularCountryName}. Cities:");

            foreach (var city in result.MostPopularCountryCities)
            {
                writer.WriteLine($"{city};");
            }
        }
        else
        {
            writer.WriteLine("Ip processing result is null. Check log for errors");
        }
    }
}