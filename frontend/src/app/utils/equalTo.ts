import { AbstractControl, FormControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function equalTo(value: FormControl): ValidatorFn {

    return (control: AbstractControl): ValidationErrors | null => {
        if (value.value != control.value) {
            return { 'equalTo': true };
        }

        return null;
    }
}
