declare module server {
	interface achievementVM {
		achievementId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		/** * From AchievementDType******************************************************************************************** */
		id: string;
		type: string;
		achievementType: string;
		creditsAvailable?: number;
		description: string;
		humanCode: string;
		identifiers: any[];
		name: string;
		fieldOfStudy: string;
		image: string;
		level: string;
		specialization: string;
		tags: string[];
		additionalProperties: { [index: string]: any };
		/** * End From AchievementDType******************************************************************************************** */
		issuer: server.profileVM;
		requirement: {
		/** Primary key. */
			criteriaId: number;
		/** IBaseEntity properties */
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
			id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
			type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
			narrative: string;
		/** Additional properties of the object */
			additionalProperties: { [index: string]: any };
		/** End From CriteriaDTypeRelationships */
			achievement: server.achievementModel;
		};
	}
}
