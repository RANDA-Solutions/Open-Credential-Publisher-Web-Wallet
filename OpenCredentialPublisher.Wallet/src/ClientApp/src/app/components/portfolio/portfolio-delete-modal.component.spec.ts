import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioDeleteModalComponent } from './portfolio-delete-modal.component';

describe('PortfolioDeleteModalComponent', () => {
  let component: PortfolioDeleteModalComponent;
  let fixture: ComponentFixture<PortfolioDeleteModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioDeleteModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioDeleteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
