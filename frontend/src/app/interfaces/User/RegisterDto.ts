export interface RegisterDto {
  username: string;
  email: string;
  password: string;
  fullname: string;
  dateOfBirth: Date;
  gender: string;
  city: string;
  imageFile?: File;
  type?: string;
  phoneNumber: string;
}
