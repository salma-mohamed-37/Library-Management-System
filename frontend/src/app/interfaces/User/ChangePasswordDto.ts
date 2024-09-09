export interface ChangePasswordDto
{
  userId?:string;
  newPassword:string;
  oldPassword:string;
}