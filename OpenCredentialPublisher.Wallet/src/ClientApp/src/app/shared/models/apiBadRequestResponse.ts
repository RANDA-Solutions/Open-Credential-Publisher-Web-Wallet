import { ApiResponse } from "./apiResponse";

export class ApiBadRequestResponse extends ApiResponse {
  errors: string[];
  constructor (statusCode: number, message: string, errors: string[]) {
    super(statusCode, message, null);
    this.errors = errors;
  }
}
