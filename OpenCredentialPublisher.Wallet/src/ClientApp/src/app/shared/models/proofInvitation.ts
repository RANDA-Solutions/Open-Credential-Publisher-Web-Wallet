export class ProofInvitation {
	credentialName: string;
	invitationId: string;
	invitationLink: string;
	shortInvitationLink: string;
	qrCodeUrl: string;
	payload: string;
}

export class ProofRequest {
	credentialName: string;
    proofRequestName: string;
	notificationAddress: string;
}