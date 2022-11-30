import { Injectable } from "@angular/core";
import { PdfRequestTypeEnum } from "@shared/models/enums/pdfRequestTypeEnum";

@Injectable({
  providedIn: "root",
})
export class EvidenceService {
  private _requestType: PdfRequestTypeEnum | null;
  private _accessKey: string | null;
  private _linkId: string | null;
  constructor() {
    this._requestType = null;
    this._accessKey = null;
    this._linkId = null;
  }

  public setLinkId(linkId) {
    this._linkId = linkId;
  }

  public setAccessKey(accessKey) {
    this._accessKey = accessKey;
  }

  public setRequestType(requestType) {
    this._requestType = requestType;
  }

  get accessKey() {
    return this._accessKey;
  }
  get requestType() {
    return this._requestType;
  }
  get linkId() {
    return this._linkId;
  }
}
