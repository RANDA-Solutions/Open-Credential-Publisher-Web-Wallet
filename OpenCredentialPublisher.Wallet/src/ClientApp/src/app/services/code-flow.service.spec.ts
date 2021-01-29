import { TestBed } from '@angular/core/testing';

import { CodeFlowService } from './code-flow.service';

describe('CodeFlowService', () => {
  let service: CodeFlowService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CodeFlowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
