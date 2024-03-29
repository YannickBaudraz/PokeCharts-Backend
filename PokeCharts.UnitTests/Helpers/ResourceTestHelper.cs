using System.Reflection;

namespace PokeCharts.UnitTests.Helpers;

public static class ResourceTestHelper
{
    internal static string GetGraphQlQuery(string fileName)
    {
        var queryResourcePath = $"GraphQlQueries.{fileName}";
        return GetResourceContent(queryResourcePath);
    }

    private static string GetResourceContent(string resourcePath)
    {
        var resourceFullPath = $"PokeCharts.UnitTests.Resources.{resourcePath}";
        Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullPath)
                                ?? throw new Exception($"Resource {resourceFullPath} not found");

        using Stream stream = resourceStream;
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}