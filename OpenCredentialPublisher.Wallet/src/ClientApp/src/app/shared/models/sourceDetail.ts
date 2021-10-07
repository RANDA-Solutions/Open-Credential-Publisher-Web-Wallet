import { SourceClr } from "./sourceClr";

export class SourceDetail {
  id: string;
  name: string;
  sourceUrl: string;
  sourceType: string;
  sourceIsDeletable: boolean;
  clrs: SourceClr[];
  constructor() {
    this.clrs = [] as Array<SourceClr>
  }
}
