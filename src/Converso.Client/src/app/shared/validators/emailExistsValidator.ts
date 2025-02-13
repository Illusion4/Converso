import { AbstractControl, AsyncValidatorFn } from "@angular/forms";
import { UserService } from "../../core/services/user.service";
import { debounce, debounceTime, delay, distinctUntilChanged, map, of, switchMap } from "rxjs";

export function emailExistsValidator(userService: UserService): AsyncValidatorFn {
    return (control: AbstractControl) => of(control.value).pipe(
        delay(500),
        switchMap((value) => userService.checkEmail(value)),
        map((result) => (result ? { emailAlreadyExists: true } : null))
    );
}