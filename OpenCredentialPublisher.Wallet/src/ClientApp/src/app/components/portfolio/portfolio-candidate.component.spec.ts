import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioCandidateComponent } from './portfolio-candidate.component';

describe('PortfolioCandidateComponent', () => {
  let component: PortfolioCandidateComponent;
  let fixture: ComponentFixture<PortfolioCandidateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioCandidateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioCandidateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
