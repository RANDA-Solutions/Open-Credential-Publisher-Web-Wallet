import { AddressDType } from "./clrLibrary/addressDType";
import { ProfileDType } from "./clrLibrary/profileDType";

export class ClrProfileVM {
  id: string;
  name: string;
  email: string;
  telephone: string;
  studentId: string;
  sourcedId: string;
  url: string;
  address: AddressDType;
  additionalProperties: { [index: string]: any };
  constructor(profile: ProfileDType){
    if (profile == null)
    {
      this.address = new AddressDType();
      this.additionalProperties = {}
      this.email = '';
      this.telephone = '';
      this.id = '';
      this.name = '';
      this.sourcedId = '';
      this.studentId = '';
      this.url = '';
    }
    else
    {
      this.address = profile.address;
      this.additionalProperties = profile.additionalProperties;
      this.email = profile.email;
      this.telephone = profile.telephone;
      this.id = profile.id;
      this.name = profile.name;
      this.sourcedId = profile.sourcedId;
      this.studentId = profile.studentId;
      this.url = profile.url;
    }
  }
}
