export interface UserDto {
  id:string;
  username: string;
  email: string;
  fullname: string;
  dateOfBirth: Date;
  gender: string;
  city: string;
  imagePath: string;
  isDeleted: boolean;
}
