declare module server {
	interface associatedAssertion extends assertionDType {
		childAssertions: server.associatedAssertion[];
		clrId?: number;
		isSelfPublished: boolean;
		assertionId: number;
		parentAssertion: server.associatedAssertion;
	}
}
