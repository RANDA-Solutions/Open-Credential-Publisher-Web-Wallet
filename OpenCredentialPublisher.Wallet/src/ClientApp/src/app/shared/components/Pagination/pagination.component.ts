import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { PaginationService } from './pagination.service';


@Component({
  selector: 'app-pagination',
  templateUrl: 'pagination.component.html',
  styleUrls: ['pagination.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})

export class PaginationComponent implements OnInit, OnChanges {
  @Input() totalCount = 0;
  @Input() currentPage = 1;
  @Input() pageSize = 0;
  @Input() pageSizes = Array<number>();
  @Output() pageChanged = new EventEmitter<any>();

  private _totalCount = 0;
  private _currentPage = 1;
  private _pagSize = 15;
  constructor(private pagerService: PaginationService) {
    this.setPage(1);
  }

  // pager object
  pager: any = {};

  ngOnInit() {
    // initialize to page 1
  }

  setPage(page: number) {
    // get pager object from service
    this.pager = this.pagerService.getPager(this._totalCount, page, this.pageSize);

    if (page < 1 || page > this.pager.totalPages) {
      return;
    }

    if (this.currentPage !== page) {
        this.pageChanged.emit({pageIndex: page, pageSize: this.pageSize});
    }
  }

  setPageSize(event: any) {
    this.pager = this.pagerService.getPager(this._totalCount, this.currentPage, event.value);

    if (event.value < 1 ) {
        return;
    }

    if (this._pagSize !== event.value) {
        this.pageChanged.emit({pageIndex: this.currentPage, pageSize: event.value});
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['totalCount']) {
      const val = changes['totalCount'].currentValue;
      this._totalCount = val;
    }
    if (changes['currentPage']) {
      const val = changes['currentPage'].currentValue;
      this._currentPage = val;
    }
    if (changes['totalCount'] || changes['currentPage']) {
      this.setPage(this._currentPage);
    }
  }
}

