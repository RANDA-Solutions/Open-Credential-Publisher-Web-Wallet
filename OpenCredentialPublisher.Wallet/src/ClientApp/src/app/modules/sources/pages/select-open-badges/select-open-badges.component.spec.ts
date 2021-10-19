import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectOpenBadgesComponent } from './select-open-badges.component';

describe('OpenBadgesComponent', () => {
  let component: SelectOpenBadgesComponent;
  let fixture: ComponentFixture<SelectOpenBadgesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectOpenBadgesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectOpenBadgesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
