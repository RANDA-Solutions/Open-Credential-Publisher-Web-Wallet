import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrComponent } from './clr.component';

describe('ClrComponent', () => {
  let component: ClrComponent;
  let fixture: ComponentFixture<ClrComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
