import { ProfileVM } from "./clrSimplified/profileVM";
import { StatusEnum } from "./enums/statusEnum";

export class ClrVM {
  id = -1;
  packageId = -1;
  name = '';
  authorizationId = '';
  publisherName = '';
  isRevoked = false;
  isSelected = false;
  issuedOn: Date;
  refreshedAt: Date;
  assertionsCount: number;
  isCollapsed = false;
  identifier = '';
  achievementIds: string[];
  enableSmartResume: boolean = false;
  hasSmartResume: boolean = false;
  smartResumeUrl: string = '';
  smartResumeStatus: StatusEnum | null;
  smartResumeMessage: string = '';
  learner: ProfileVM;
  publisher: ProfileVM;
  constructor(){
    this.id = -1;
    this.packageId = -1;
    this.name = '';
    this.authorizationId = '';
    this.publisherName = '';
    this.isRevoked = false;
    this.isSelected = false;
    this.issuedOn = null;
    this.refreshedAt = null;
    this.assertionsCount = -1;
    this.isCollapsed = false;
    this.identifier = '';
    this.achievementIds = new Array<string>();
    this.learner = null;
    this.publisher = null;
    this.enableSmartResume = false;
    this.hasSmartResume = false;
    this.smartResumeStatus = null;
    this.smartResumeMessage = null;
    this.smartResumeUrl = '';
  }
}
