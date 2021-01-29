import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { MainSettings } from 'src/main.constants';

@UntilDestroy()
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  isExpanded = false;
  private _isAuthenticated: BehaviorSubject<Boolean> = new BehaviorSubject(false);
  public isAuthenticated$: Observable<Boolean>|false = this._isAuthenticated.asObservable();

  private _showPortfolioBadge: BehaviorSubject<Boolean> = new BehaviorSubject(false);
  public showPortfolioBadge$: Observable<Boolean>|false = this._showPortfolioBadge.asObservable();

  private _portfolioBadge: BehaviorSubject<Number> = new BehaviorSubject(0);
  public portfolioBadge$:Observable<Number>|0 = this._portfolioBadge.asObservable();

  public SiteName = MainSettings.SiteName;

  constructor(private authorizeService: AuthorizeService) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().pipe(untilDestroyed(this)).subscribe(v => {
      this._isAuthenticated.next(v);
    })
  }

  ngOnDestroy() {
    
  }
}
