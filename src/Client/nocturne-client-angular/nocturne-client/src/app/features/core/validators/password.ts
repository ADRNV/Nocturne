import { AbstractControl, FormControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class PasswordValidator{
    
    static passwordEqualityValidator(passwordControl: FormControl): ValidatorFn{

    return (control: AbstractControl): ValidationErrors | null => {
            
            let previusPassword = null;
            let result = null;
            result = {passwordNotEqual: true};
            //Unsub ?
           let sub = (<FormControl>control).valueChanges.subscribe((value) => {
                previusPassword = value === null ? ' ' : value;
                
                if(passwordControl.value != previusPassword){ 
                    console.log(true);
                    result = {passwordNotEqual: true};
                }
                else {
                    console.log(false);
                    result = null;
                }
            });

            return result;
        }
    }
}