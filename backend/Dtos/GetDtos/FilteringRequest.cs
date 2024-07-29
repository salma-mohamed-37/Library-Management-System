namespace backend.Dtos.GetDtos
{
    public class FilteringRequest
    {
        public string? Name {set; get;}
        public string? Author {set; get;}
        public string? Category {set; get;}
        public DateTime? FromDate {set; get;}
        public DateTime? ToDate {set; get;}
        public string? SortField {set; get;}
        public string? SortDirection {set; get;}
        public int PageNumber {set; get;}
        public int PageSize {set; get;}
    }
}