export interface Book {
  id: number;
  name: string;
  publishDate: Date;
  categoryName: string;
  authorName: string;
  imagePath: string;
  currentlyBorrowed: boolean;
  borrowDate?: Date;
  dueDate?: Date;
  returnDate?: Date;
}
