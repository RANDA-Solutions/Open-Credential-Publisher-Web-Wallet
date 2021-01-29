import { Component, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { CandidateService } from "src/app/services";


@UntilDestroy()
@Component({
  selector: 'app-portfolio',
  templateUrl: './portfolio.component.html',
  styleUrls: ['./portfolio.component.css']
})
export class PortfolioComponent implements OnInit {

    constructor(private candidateService: CandidateService ) {

    }

    ngOnInit() {
        this.candidateService.get().pipe(untilDestroyed(this)).subscribe();
    }
}