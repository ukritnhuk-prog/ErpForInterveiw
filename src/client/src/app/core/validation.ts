import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const nonBlank: ValidatorFn = (control: AbstractControl): ValidationErrors | null =>
  typeof control.value === 'string' && control.value.trim() ? null : { required: true };

export function localToday(): string {
  const today = new Date();
  return [today.getFullYear(), String(today.getMonth() + 1).padStart(2, '0'), String(today.getDate()).padStart(2, '0')].join('-');
}

export const employmentDates: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const birth = control.get('dateOfBirth')?.value;
  const joined = control.get('dateJoined')?.value;
  const today = localToday();
  if ((birth && birth > today) || (joined && joined > today)) return { futureDate: true };
  if (birth && joined && joined <= birth) return { dateOrder: true };
  return null;
};
