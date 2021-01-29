import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { HomeModel } from 'src/app/models/home.model';
import { HomeService } from 'src/app/services/home.service';

@UntilDestroy()
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit, OnDestroy {
  private _isAuthenticated = new BehaviorSubject<boolean>(false);
  public isAuthenticated$: Observable<boolean> = this._isAuthenticated.asObservable();
  private _modelSubject = new BehaviorSubject<HomeModel>(null);
  public model$: Observable<HomeModel> = this._modelSubject.asObservable();
  constructor(private authorizeService: AuthorizeService, private homeService: HomeService) {}

  ngOnInit() {
    this.authorizeService.isAuthenticated().pipe(untilDestroyed(this)).subscribe(a => {
      console.log("is authenticated" + a);
      this._isAuthenticated.next(a);
      if (a) {
        this.homeService.get().subscribe(model => {
          this._modelSubject.next(model);
        });
      }
    });
  }

  ngOnDestroy() {}
}
