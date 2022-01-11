import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { UserInfoViewModel } from 'src/app/services/view-models/user-info-view-model';
import { LocalStorageUtil } from 'src/app/utils/local-storage-util';

@Component({
  selector: 'app-user',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  localStorageUtil: LocalStorageUtil = new LocalStorageUtil();
  userLogin = this.localStorageUtil.getUser();
  users: UserInfoViewModel[] = [];

  form: FormGroup = new FormGroup({
    name: new FormControl(''),
  });

  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.userService.getAll(this.form.controls['name'].value).subscribe(
      (response) => {
        this.users = response == null ? [] : response.data
      },
      (error) => {
        console.error(error);
      }
    );
  }

  remove(id: string) {
    this.userService.delete(id).subscribe(
      (response) => {
        alert(response.messages[0]);
        this.getUsers();
      },
      (error) => {
        alert(error.messages[0]);
        console.error(error);
      }
    );
  }
}
