import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment/environment';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { PdfRequest } from '@shared/models/pdfRequest';
import * as FileSaver from 'file-saver';
import { UtilsService } from './utils.service';

@Injectable({
  providedIn: 'root'
})
export class DownloadService {
    private debug = false;

    constructor(private http: HttpClient, private utilsService: UtilsService) { }

    pdf(input: PdfRequest) {
      let urlApi = `${environment.apiEndPoint}Downloads/Pdf`;
      if (input.requestType == PdfRequestTypeEnum.LinkViewPdf ) {
        urlApi = `${environment.apiEndPoint}Public/Links/Pdf`;
      }
      if (this.debug) console.log(`DownloadService service ${urlApi}`);
      return this.http.post(urlApi, input, {responseType: 'blob', observe: 'response'});
    }
    vcJson(id: number) {

      const urlApi = `${environment.apiEndPoint}Downloads/VCJson/${id}`;
      if (this.debug) console.log(`DownloadService service ${urlApi}`);
      return this.http.post(urlApi, null, {responseType: 'json', observe: 'response'});
    }
    vcLinkJson(id: string, key: string = null) {
      const urlApi = `${environment.apiEndPoint}Public/Links/DownloadVCJson/${id}`;
      if (this.debug) console.log(`DownloadService service ${urlApi}`);
      return this.http.post(urlApi, JSON.stringify(key), {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
        , responseType: 'json', observe: 'response'
      });
    }

    public saveAs(data: any, fileName: string, type: string) {
      let blob: Blob;
      let file: File;
      if (type === 'Excel') {
        blob = new Blob([data],
          { type: 'application/vnd.ms-excel' });
        file = new File([blob], fileName,
          { type: 'application/vnd.ms-excel' });
      } else if (type === 'Pdf') {
        blob = new Blob([data],
          { type: 'application/pdf' });
        file = new File([blob], fileName,
          { type: 'application/pdf' });
      } else {
        blob = new Blob([data],
          { type: 'text/comma-separated-values' });
        file = new File([blob], fileName,
          { type: 'text/comma-separated-values' });
      }

      FileSaver.saveAs(file);
    }
  }
