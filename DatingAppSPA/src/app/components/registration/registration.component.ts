import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PasswordValidator } from 'src/app/shared/passwordvalidator';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  form: FormGroup = new FormGroup({
    email: new FormControl("", [Validators.required,Validators.email]),
    name: new FormControl("", Validators.required),
    userName: new FormControl("", Validators.required),
    password: new FormControl("", Validators.required),
    cpassowrd:new FormControl("",Validators.required)
    
  }, { validators: PasswordValidator });
  
  /**
   *
   */
 
  get email() {
    return this.form.get("email");
  }
  get name() {
    return this.form.get("name");

  }
  get userName() {
    return this.form.get("userName");
  }
  get password() {
    return this.form.get("password");
  }
  get cpassword() {
    return this.form.get("cpassowrd");
  }

 
  constructor(private auth:AuthService , private toster:ToastrService) { }

  ngOnInit(): void {
  }

  SubmitForm() {
    this.auth.Registration(this.form.value).subscribe(a => {
      // go to login 
    }, (error) => {
       
        this.toster.error(error.error, "Error");
        
    }, () => {
        console.clear();
    });

  }

}
