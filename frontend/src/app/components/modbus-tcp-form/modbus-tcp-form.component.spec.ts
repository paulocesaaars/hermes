import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModbusTcpFormComponent } from './modbus-tcp-form.component';

describe('ModbusTcpFormComponent', () => {
  let component: ModbusTcpFormComponent;
  let fixture: ComponentFixture<ModbusTcpFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModbusTcpFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModbusTcpFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
