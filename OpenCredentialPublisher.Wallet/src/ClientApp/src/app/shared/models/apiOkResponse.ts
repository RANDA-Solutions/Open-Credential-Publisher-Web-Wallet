import { ApiResponse } from "./apiResponse";

export class ApiOkResult<T> extends ApiResponse {
  result: T;
  constructor (result: T ) {
    super(200, 'Ok', null);
    this.result = result;
  }
}

export class ApiOkResponse extends ApiResponse {
  result: any;
  constructor (result ) {
    super(200, 'Ok', null);
    this.result = result;
  }
}
