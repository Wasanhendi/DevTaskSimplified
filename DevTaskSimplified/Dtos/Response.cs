namespace DevTaskSimplified.Dtos;

public class Response
{
    public required int Count { get; set; }
    public required string Message { get; set; }
    public required string SearchCriteria { get; set; }
    public required IEnumerable<Results> Results { get; set; }
}