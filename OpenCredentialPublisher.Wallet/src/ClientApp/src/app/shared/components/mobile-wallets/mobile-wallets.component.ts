import { Component, Input, OnInit } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { MobileWalletLink } from "@shared/models/mobile-wallet-link";

@Component({
	selector: 'app-mobile-wallets',
	templateUrl: './mobile-wallets.component.html',
	styleUrls: ['./mobile-wallets.component.scss']
  })
export class MobileWalletsComponent implements OnInit {
	@Input() payload: string;
	@Input() passportLink: string;
	mobileWallets: MobileWalletLink[];
	
	constructor(private sanitizer: DomSanitizer) {
		this.mobileWallets = new Array<MobileWalletLink>();
	}

	ngOnChanges() {}

	ngOnInit() {
		this.mobileWallets.push(
			{
				title: "Connect.Me",
				url: this.sanitizer.bypassSecurityTrustUrl(`https://connectme.app.link?t=http%3A%2F%2Fvas.pps.evernym.com%3A80%2Fagency%2Fmsg%3Fc_i%3D${this.payload}`)
			},
			{
				title: "Passport",
				url: this.passportLink ?? this.sanitizer.bypassSecurityTrustUrl(`https://oob.idramp.com/?c_i=${this.payload}`)
			},
			{
				title: "Trinsic Wallet",
				url: this.sanitizer.bypassSecurityTrustUrl(`id.streetcred://launch?c_i=${this.payload}`)
			}
		)
	}

}