import { TestBed, inject } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';

describe('AuthorizeGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthGuard]
    });
  });

  it('should ...', inject([AuthGuard], (guard: AuthGuard) => {
    expect(guard).toBeTruthy();
  }));
});
