import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-modbus-rtu-view',
  templateUrl: './modbus-rtu-view.component.html',
  styleUrls: ['./modbus-rtu-view.component.scss']
})
export class ModbusRtuViewComponent implements OnInit {

  @Input() device?: any;

  constructor() { }

  ngOnInit(): void {
  }

}
