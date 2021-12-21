import { AchievementResult } from "./achievementResult";

export class ClrAssertionVM {
  id: number;
  achievementName: string;
  achievementType: string;
  achievementIssuerName: string;
  issuedOn: Date | null;
  isCollapsed: boolean;
  achievementResults: AchievementResult[];
}
