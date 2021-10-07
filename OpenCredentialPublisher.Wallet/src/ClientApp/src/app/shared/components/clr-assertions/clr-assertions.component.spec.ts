import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrAssertionsComponent } from './clr-assertions.component';

describe('ClrAssertionsComponent', () => {
  let component: ClrAssertionsComponent;
  let fixture: ComponentFixture<ClrAssertionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrAssertionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrAssertionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
