import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageUtil } from 'src/app/utils/local-storage-util';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  errorMessage: string = '';
  loginAuth: any;
  localStorageUtil: LocalStorageUtil = new LocalStorageUtil();

  form: FormGroup = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(30),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
    ]),
  });

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    this.localStorageUtil.clear();
  }

  login(): void {
    if (this.form.valid) {
      this.loginAuth = Object.assign({}, this.loginAuth, this.form.value);
      this.authService.login(this.loginAuth).subscribe(
        (response) => {
          this.errorMessage = '';
          this.localStorageUtil.saveAll(response.data);
          this.router.navigate(['/']);
        },
        (error) => {
          this.errorMessage = error.messages[0];
          console.error(error);
        }
      );
    }
  }
}
