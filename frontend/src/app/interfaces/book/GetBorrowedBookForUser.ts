export interface GetBorrowedBookForUser
{
  id: number;
  name: string;
  imagePath: string;
  borrowDate?: Date;
  returnDate?: Date;
  dueDate?: Date;
}
