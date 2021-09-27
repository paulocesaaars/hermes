import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { LocalStorageUtil } from 'src/app/utils/local-storage-util';
import { UserInfoViewModel } from 'src/app/services/view-models/user-info-view-model';

@Component({
  selector: 'app-user',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {

  localStorageUtil: LocalStorageUtil = new LocalStorageUtil();
  userLogin = this.localStorageUtil.getUser();
  users: UserInfoViewModel[] = [];
  totalRegisters: number = 0;
  take: number = 10;
  skip: number = 0;

  form: FormGroup = new FormGroup({
    name: new FormControl(''),
  });

  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.userService
      .getAll(this.form.controls['name'].value, this.take, this.skip)
      .subscribe(
        (response) => {
          this.users = response.data;
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

  changePaginator(paginator: PageEvent) {
    this.take = (paginator.pageIndex + 1) * paginator.pageSize;
    this.skip = this.take - paginator.pageSize;
    this.getUsers();
  }
}
