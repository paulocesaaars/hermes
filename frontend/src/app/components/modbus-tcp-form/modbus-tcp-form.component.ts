import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-modbus-tcp-form',
  templateUrl: './modbus-tcp-form.component.html',
  styleUrls: ['./modbus-tcp-form.component.scss'],
})
export class ModbusTcpFormComponent implements OnInit {

  form: FormGroup = new FormGroup({
    ip: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(30),
    ]),
  });

  constructor() {}

  ngOnInit(): void {}
}
