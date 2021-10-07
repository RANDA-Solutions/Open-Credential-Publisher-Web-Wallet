import { AchievementVM } from "./achievementVM";
import { IdentityVM } from "./identityVM";
import { VerificationVM } from "./verificationVM";

export class AssertionWithAchievementVM {
  assertionId: number;
	signedAssertion: string;
	id: string;
	type : string;
	creditsEarned:number;
	activityEndDate:Date;
	eExpires:Date;
	image : string;
	issuedOn:Date;
	licenseNumber : string;
	narrative : string;
	revocationReason : string;
	revoked: boolean;
	role: string;
	activityStartDate:Date;
	term: string;
	additionalProperties: { [index: string]: any };
	isDeleted: boolean;
	createdAt:Date;
	modifiedAt:Date;
	context: string;
	isSigned: boolean;
	isSelfPublished: boolean;
	achievement: AchievementVM;
  recipient: IdentityVM;
  verification: VerificationVM;
}
