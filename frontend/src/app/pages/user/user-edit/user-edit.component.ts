import { UserInfoViewModel } from '../../../services/view-models/user-info-view-model';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss'],
})
export class UserEditComponent implements OnInit {
  enabled = false;
  administrator = false;
  user?: UserInfoViewModel;

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
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService
  ) {
    this.user = this.route.snapshot.data['user'].data;
    this.enabled = this.user?.enabled ?? false;
    this.administrator = this.user?.administrator ?? false;
  }

  ngOnInit(): void {
    this.form.patchValue({
      id: this.user?.id ?? '',
      userName: this.user?.userName ?? '',
      fullName: this.user?.fullName ?? '',
    });
  }

  save() {
    if (this.form.valid) {
      let newUser = Object.assign({}, this.form.value);
      newUser.id = this.user?.id;
      newUser.enabled = this.enabled;
      newUser.administrator = this.administrator;

      this.userService.put(newUser).subscribe(
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
