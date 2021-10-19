import { OpenBadge } from "./openBadge";

export class BackpackPackage {
  id: number;
  badges: OpenBadge[];
  isBadgr: boolean;
  constructor() {
    this.id = 0;
    this.isBadgr = true;
    this.badges = new Array<OpenBadge>();
  }
}
