import { PackageVM } from "./packageVM";

export class PackageList {
  modelIsValid: boolean;
  enableSource: boolean;
  enableCollections: boolean;
  modelErrors: string[];
  userId: string;
  packages: PackageVM[];
  constructor () {
    this.modelIsValid = true;
		this.enableSource = true;
		this.enableCollections = true;
    this.modelErrors = new Array<string>();
    this.userId = '';
    this.packages = new Array<PackageVM>();
  }
}
