import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { CodeService } from "@modules/access/services/code.service";

@Component({
  selector: 'app-code-credential',
  templateUrl: './code-credential.component.html',
  styleUrls: ['./code-credential.component.scss']
})
export class CodeCredentialComponent implements OnInit {
  constructor(private codeService: CodeService, private router: Router, private activatedRoute: ActivatedRoute) {

  }

  ngOnInit(): void {

  }
}
