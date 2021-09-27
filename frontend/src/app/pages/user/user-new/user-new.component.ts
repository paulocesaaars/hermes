import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { equalTo } from 'src/app/utils/equalTo';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styleUrls: ['./user-new.component.scss'],
})
export class UserNewComponent implements OnInit {
  enabled = false;

  password = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(10),
  ]);
  confirmPassword = new FormControl('', [
    Validators.required,
    equalTo(this.password),
  ]);

  form: FormGroup = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(30),
    ]),
    fullName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(100),
    ]),
    password: this.password,
    confirmPassword: this.confirmPassword,
  });

  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {}

  save() {
    if (this.form.valid) {
      let user = Object.assign({}, this.form.value);

      user.enabled = this.enabled;

      this.userService.post(user).subscribe(
        () => {
          this.router.navigate(['/user']);
        },
        (error) => {
          alert(error.messages[0]);
          console.error(error);
        }
      );
    }
  }
}
