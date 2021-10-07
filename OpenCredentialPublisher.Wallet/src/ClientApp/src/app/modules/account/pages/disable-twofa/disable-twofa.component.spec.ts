import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisableTwofaComponent } from './disable-twofa.component';

describe('DisableTwofaComponent', () => {
  let component: DisableTwofaComponent;
  let fixture: ComponentFixture<DisableTwofaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DisableTwofaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DisableTwofaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
