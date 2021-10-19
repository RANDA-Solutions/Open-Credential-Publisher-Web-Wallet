import { ApiResponse } from "./apiResponse";

export class ApiOkResponse extends ApiResponse {
  result: any;
  constructor (result ) {
    super(200, 'Ok', null);
    this.result = result;
  }
}
