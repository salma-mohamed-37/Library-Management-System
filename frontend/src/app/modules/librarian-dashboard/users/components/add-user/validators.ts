import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function dateValidator() : ValidatorFn
{
  return (control: AbstractControl): ValidationErrors | null => {
    const dateOfBirth = new Date(control.value);
    const today = new Date();

    // Check if the date of birth is in the future
    if (dateOfBirth > today) {
      return { 'futureDate': true }; // Invalid, date is in the future
    }

    return null; // Valid
  };
}

export function passwordMatch(passwordKey:string, confirmPasswordKey:string)
{
  return (control: AbstractControl): ValidationErrors | null => {
    const password = control.get(passwordKey)?.value;
    const confirmPassword = control.get(confirmPasswordKey)?.value;

    if (password !== confirmPassword) {
      return { 'passwordMismatch': true };
    }
    return null;
  };
}
