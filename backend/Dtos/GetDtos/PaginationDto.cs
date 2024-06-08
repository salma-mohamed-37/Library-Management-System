namespace backend.Dtos.GetDtos
{
    public class PaginationDto<T>
    {
        public int PageSize { set; get; }
        public int PageNumber { set; get; }
        public int Count { set; get; }
        public ICollection<T> Data { set; get; }
    }
}
