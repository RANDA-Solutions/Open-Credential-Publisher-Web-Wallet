/* eslint-disable @typescript-eslint/no-unused-vars */
import { APP_BASE_HREF, Location, PopStateEvent } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { AppService } from '@core/services/app.service';
import { AuthService } from '@core/authentication/auth.service';
import { CoreModule } from '@core/core.module';
import { AuthGuard } from '@core/guards/auth.guard';
import { MockAuthGuard } from '@mocks/guards/auth.guard.spec';
import { MockAppService } from '@mocks/services/app.service.spec';
import { MockAuthService } from '@mocks/services/auth.service.spec';
import { CourseModule } from './course.module';
import { courseRoutes } from './course.router';


describe('Router: courses', () => {

  let location: Location;
  let router: Router;
  // let fixture;
  let lastPath: PopStateEvent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
          BrowserModule,
        CoreModule,
        CourseModule,
        RouterTestingModule.withRoutes(courseRoutes)
    ],
      declarations: [
          // PageNotFoundComponent
      ],
      providers: [
        {provide: APP_BASE_HREF, useValue: '/'},
        { provide: AppService, useClass: MockAppService },
        { provide: AuthService, useClass: MockAuthService },
        { provide: AuthGuard, useClass: MockAuthGuard }
      ]
    });

    router = TestBed.get(Router);
    location = TestBed.get(Location);

    location.subscribe(val => {
      lastPath = val;
    });

    router.initialNavigation();
  });

  it('navigate to "" takes you to /courselist', (done) => {
    router.navigateByUrl('')
    // router.navigate([''])
    .then(() => {
      expect(location.path()).toBe('/courselist');
      done();
    });
  });
  it('navigate to "/courselist" takes you to /courselist', (done) => {
    router.navigate(['courselist'])
    .then(() => {
      expect(location.path()).toBe('/courselist');
      done();
    });
  });
  it('navigate to "/editcourse/-1" takes you to /editcourse/-1', (done) => {
    router.navigate(['editcourse', '-1'])
    .then(() => {
      expect(location.path()).toBe('/editcourse/-1');
      done();
    });
  });
});
