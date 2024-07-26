export interface PaginationDto<T> {
  count: number;
  pageNumber: number;
  pageSize: number;
  data: T[];
}
