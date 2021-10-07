export module server {
	/** The type of association. This uses an enumerated vocabulary. */
	const enum associationTypeEnum {
		/** Enum ExactMatchOfEnum for exactMatchOf */
		exactMatchOfEnum = 1,
		/** Enum ExemplarEnum for exemplar */
		exemplarEnum = 2,
		/** Enum HasSkillLevelEnum for hasSkillLevel */
		hasSkillLevelEnum = 3,
		/** Enum IsChildOfEnum for isChildOf */
		isChildOfEnum = 4,
		/** Enum IsParentOfEnum for isParentOf */
		isParentOfEnum = 5,
		/** Enum IsPartOfEnum for isPartOf */
		isPartOfEnum = 6,
		/** Enum IsPeerOfEnum for isPeerOf */
		isPeerOfEnum = 7,
		/** Enum IsRelatedToEnum for isRelatedTo */
		isRelatedToEnum = 8,
		/** Enum PrecedesEnum for precedes */
		precedesEnum = 9,
		/** Enum ReplacedByEnum for replacedBy */
		replacedByEnum = 10,
	}
	/** Association is based on the CASE AssociationLink object. An Association associates (relates) one Achievement with another Achievement. */
	interface associationDType {
		/** The type of association. This uses an enumerated vocabulary. */
		associationType: any;
		/** The '@id' of another achievement, or target of the association. Model Primitive Datatype = NormalizedString. */
		targetId: string;
		/** A human readable title for the associated achievement. Model Primitive Datatype = String. */
		title: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
