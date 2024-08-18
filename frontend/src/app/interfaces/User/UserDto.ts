export interface UserDto {
  id:string;
  username: string;
  email: string;
  fullname: string;
  dateOfBirth: Date;
  gender: string;
  phoneNumber:string;
  city: string;
  imagePath: string;
  isDeleted?: boolean;
  deletedAt?:Date;
}
