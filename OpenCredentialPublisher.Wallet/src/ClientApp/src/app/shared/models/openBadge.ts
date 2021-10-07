export class OpenBadge {
  id: number;
  badgrAssertionId: string;
  badgeName: string;
  issuerName: string;
  badgeDescription: string;
  badgeImage: string;
  idIsUrl: boolean;
  constructor() {
    this.id = 0;
    this.badgrAssertionId = '';
    this.badgeName = '';
    this.issuerName = '';
    this.badgeDescription = '';
    this.badgeImage = '';
    this.idIsUrl = true;
  }
}
