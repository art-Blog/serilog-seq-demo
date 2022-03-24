namespace serilog_seq_demo.DataClass.Record;

public record AdminRecord
{
    public string Id { get; init; } = string.Empty;
    public string Account { get; init; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? Authority { get; init; }
}