import { StatusEnum } from "./statusenum.model";

export class CandidateModel {
    candidateId: string;
    passedVerification: boolean;
    verificationDate: Date;
    lastCheckedDate: Date;
    hasTests: boolean;
    status: StatusEnum;
    tests: any[];
    totalTests: number;
    lastReportedScore: string;
    isDeleted: boolean;
    firstName: string;
    lastName: string;
    dob: string;
    totalCredits: number;
}