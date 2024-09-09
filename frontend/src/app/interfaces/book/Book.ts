export interface Book {
  id: number;
  name: string;
  publishDate: Date;
  category_name: string;
  author_name: string;
  imagePath: string;
  currently_borrowed: boolean;
  borrowDate?: Date;
  dueDate?: Date;
  returnDate?: Date;
}
