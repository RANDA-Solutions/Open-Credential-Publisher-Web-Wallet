import { ClrVM } from "./clrVM";

export class ClrCollectionVM {
  name: string;
  clrs: Array<ClrVM>;
  constructor(){
    this.clrs = [] as Array<ClrVM>;
  }
}
