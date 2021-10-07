export class ProofResponse {
	verificationResult: string;
	selfAttestedAttributes: string;
	revealedAttributes: ProofRevealedAttributes;
	predicates: string;
	unrevealedAttributes: string;
	identifiers: ProofResponseIdentifier[];
}

export class ProofResponseIdentifier {
	schema_id: string;
	cred_def_id: string;

	get schemaName()  {
		return this.schema_id.replace(/\w+:\d+:([\w\s]+):[\w\.]+/g, '$1').trim();
	}

	get schemaVersion()  {
		return this.schema_id.replace(/\w+:\d+:[\w\s]+:([\w\.]+)/g, '$1').trim();
	}
}

export interface AttachmentAttribute {
	identifier_index: number;
	value: AttachmentMetadata;
}

export interface AttachmentMetadata {
	'mime-type': string;
	extension: string;
	name: string;
	data: AttachmentData;
}

export interface AttachmentData {
	base64: string;
}

export interface ProofRevealedAttributes {
	[key: string]: string | AttachmentAttribute | number;
}