import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-modbus-tcp-view',
  templateUrl: './modbus-tcp-view.component.html',
  styleUrls: ['./modbus-tcp-view.component.scss']
})
export class ModbusTcpViewComponent implements OnInit {

  @Input() device?: any;

  constructor() { }

  ngOnInit(): void {
  }

}
