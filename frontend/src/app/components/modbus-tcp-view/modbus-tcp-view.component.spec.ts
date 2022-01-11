import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModbusTcpViewComponent } from './modbus-tcp-view.component';

describe('ModbusTcpViewComponent', () => {
  let component: ModbusTcpViewComponent;
  let fixture: ComponentFixture<ModbusTcpViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModbusTcpViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModbusTcpViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
