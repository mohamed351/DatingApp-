import { AbstractControl } from "@angular/forms";

export function PasswordValidator(control: AbstractControl): {[key:string]:boolean}|null {
    const ConfirmPassword = control.get("cpassowrd");
    const Password = control.get("password");
    console.log(ConfirmPassword);
    console.log(Password);
    return ConfirmPassword && Password && Password.value != ConfirmPassword.value ? {"misMatch":true }:null
}