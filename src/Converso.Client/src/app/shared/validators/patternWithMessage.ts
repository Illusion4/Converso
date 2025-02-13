import { AbstractControl, ValidatorFn, Validators } from "@angular/forms";

export const patternWithMessage = (pattern: string | RegExp, message: string): ValidatorFn => {
    const delegateFn = Validators.pattern(pattern);
    return (control: AbstractControl) => {
      const result = delegateFn(control);
      return result === null ? null : { pattern: { message } };
    };
  };