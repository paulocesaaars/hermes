import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModbusTcpDataComponent } from './modbus-tcp-data.component';

describe('ModbusTcpViewComponent', () => {
  let component: ModbusTcpDataComponent;
  let fixture: ComponentFixture<ModbusTcpDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModbusTcpDataComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModbusTcpDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
