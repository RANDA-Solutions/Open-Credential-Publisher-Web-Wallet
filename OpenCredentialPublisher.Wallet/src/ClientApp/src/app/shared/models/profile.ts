export class Profile {
  hasProfileImage: boolean;
  profileImageUrl: string;
  displayName: string;
  missingDisplayName: boolean;
  credentials: number;
  achievements: number;
  scores: number;
  activeLinks: number;
  additionalData: { [index: string]: string };

  constructor() {
    this.hasProfileImage = false;
    this.profileImageUrl = '';
    this.missingDisplayName = true;
    this.displayName = '';
    this.achievements = 0;
    this.credentials = 0;
    this.activeLinks = 0;
    this.scores = 0;
    this.additionalData = {};
  }
}
