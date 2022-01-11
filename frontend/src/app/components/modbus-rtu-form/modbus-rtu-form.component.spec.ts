import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModbusRtuFormComponent } from './modbus-rtu-form.component';

describe('ModbusRtuFormComponent', () => {
  let component: ModbusRtuFormComponent;
  let fixture: ComponentFixture<ModbusRtuFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModbusRtuFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModbusRtuFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
