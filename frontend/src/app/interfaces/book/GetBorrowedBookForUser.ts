export interface GetBorrowedBookForUser
{
  id: number;
  name: string;
  publishDate: Date;
  category_name: string;
  author_name: string;
  imagePath: string;
  borrowDate?: Date;
  returnDate?: Date;
  dueDate?: Date;
}
