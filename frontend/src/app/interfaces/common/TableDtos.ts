export interface LazyLoadEventDTO
{
  first?: number|undefined;
  rows?: number|undefined|null;
  sortField?: string |null|undefined|string[];
  sortOrder?: number|null;
  filters?: { [key: string]: LazyFilterItemDTO | LazyFilterItemDTO[] |undefined }|undefined;
}

export interface LazyFilterItemDTO
{
  value?: string | null;
  matchMode?: string|undefined;
}
