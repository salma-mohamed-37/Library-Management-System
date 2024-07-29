export interface FilteringRequest
{
  name?: string;
  author?: string;
  category?: string;
  fromDate?: Date;
  toDate?: Date;
  sortField?: string;
  sortDirection?: string;
  pageNumber: number;
  pageSize: number;
}
