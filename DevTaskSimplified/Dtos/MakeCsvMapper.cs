using LINQtoCSV;

namespace DevTaskSimplified.Dtos;

public class MakeCsvMapper
{
    [CsvColumn(FieldIndex = 1)]
    public string make_id { get; set; }

    [CsvColumn(FieldIndex = 2)]
    public string make_name { get; set; }

}
