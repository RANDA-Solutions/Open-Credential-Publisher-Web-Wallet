import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpenBadgesComponent } from './open-badges.component';

describe('OpenBadgesComponent', () => {
  let component: OpenBadgesComponent;
  let fixture: ComponentFixture<OpenBadgesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OpenBadgesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OpenBadgesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
