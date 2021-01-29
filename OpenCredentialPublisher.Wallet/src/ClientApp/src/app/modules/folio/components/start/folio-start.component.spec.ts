import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolioStartComponent } from './folio-start.component';

describe('FolioStartComponent', () => {
  let component: FolioStartComponent;
  let fixture: ComponentFixture<FolioStartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolioStartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolioStartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
