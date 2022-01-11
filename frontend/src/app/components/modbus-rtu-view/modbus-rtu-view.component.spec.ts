import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModbusRtuViewComponent } from './modbus-rtu-view.component';

describe('ModbusRtuViewComponent', () => {
  let component: ModbusRtuViewComponent;
  let fixture: ComponentFixture<ModbusRtuViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModbusRtuViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModbusRtuViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
