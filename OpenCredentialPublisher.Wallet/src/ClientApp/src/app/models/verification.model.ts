import { SelectModel } from "./select.model";

export class VerificationModel {
    verificationId: number;
    verifySsn: boolean;
    error: boolean;
    errorMessage: string;
    names: SelectModel[];
    addresses: SelectModel[];
}