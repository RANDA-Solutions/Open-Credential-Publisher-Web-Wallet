import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeflowComponent } from './codeflow.component';

describe('CodeflowComponent', () => {
  let component: CodeflowComponent;
  let fixture: ComponentFixture<CodeflowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CodeflowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeflowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
