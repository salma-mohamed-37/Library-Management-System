export interface LoginDto
{
  email : string;
  password:string;
}


export interface LoggedInUser
{
  id:string;
  role:string;
  token:string;
  expiryDate:Date;
  imagePath:string;
}
