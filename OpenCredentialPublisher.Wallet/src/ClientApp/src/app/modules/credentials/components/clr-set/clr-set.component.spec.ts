import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrSetComponent } from './clr-set.component';

describe('ClrSetComponent', () => {
  let component: ClrSetComponent;
  let fixture: ComponentFixture<ClrSetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrSetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrSetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
