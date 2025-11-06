namespace SearchMvp.Api;

public sealed class EsSettings
{
    public string Uri { get; set; } = "http://localhost:9200";
    public string IndexPrefix { get; set; } = "mvp";
}