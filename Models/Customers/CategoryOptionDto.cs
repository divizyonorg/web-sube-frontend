namespace MyApp.Web.Models.Customers;

public class CategoryOptionDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class CategoryListResponseDto
{
    public bool Success { get; set; }
    public string Category { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public List<CategoryOptionDto> Data { get; set; } = [];
}
