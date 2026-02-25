namespace TelefonOzellikleri.Configuration;

public class ElasticsearchSettings
{
    public const string SectionName = "Elasticsearch";

    public string Url { get; set; } = "http://localhost:9200";
    public string IndexName { get; set; } = "smartphones";

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Url);
}
