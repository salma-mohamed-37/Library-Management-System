export interface FilteringRequest
{
  name?: string;
  author?: string;
  category?: string;
  fromDate?: Date;
  toDate?: Date;
  sortField?: string;
  sortDirection?: string;
  IsDeleted? :boolean;
  pageNumber: number;
  pageSize: number;
}
