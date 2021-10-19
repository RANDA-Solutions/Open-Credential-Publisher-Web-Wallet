export class OpenBadge {
  id: number;
  badgrAssertionId: string;
  badgeName: string;
  issuerName: string;
  badgeDescription: string;
  badgeImage: string;
  idIsUrl: boolean;
  isSelected: boolean;
  isNew: boolean;
  constructor() {
    this.id = 0;
    this.badgrAssertionId = '';
    this.badgeName = '';
    this.issuerName = '';
    this.badgeDescription = '';
    this.badgeImage = '';
    this.idIsUrl = true;
    this.isSelected = false;
    this.isNew = false;
  }
}
