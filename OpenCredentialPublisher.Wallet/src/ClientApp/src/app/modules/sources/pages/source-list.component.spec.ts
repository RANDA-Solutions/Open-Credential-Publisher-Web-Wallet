import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SourceListComponent } from './source-list.component';

describe('SourceListComponent', () => {
  let fixture: ComponentFixture<SourceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SourceListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SourceListComponent);
    fixture.detectChanges();
  });

  it('should display a title', async(() => {
    const titleText = fixture.nativeElement.querySelector('h1').textContent;
    expect(titleText).toEqual('Counter');
  }));

});
