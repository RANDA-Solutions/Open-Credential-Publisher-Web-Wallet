import { AssertionDType } from "./clrLibrary/assertionDType";

export class AssociatedAssertion extends AssertionDType {
  assertionId: number;
  childAssertions: AssociatedAssertion[];
  clrId?: number;
  isSelfPublished?: boolean;
  parentAssertion: AssociatedAssertion;
}
