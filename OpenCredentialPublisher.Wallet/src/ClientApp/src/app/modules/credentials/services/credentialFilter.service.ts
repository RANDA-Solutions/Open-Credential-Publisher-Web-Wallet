import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CredentialFilterService {
  private localStorageFilterKey = "credentialpackagefilter";

  constructor() { }

  setFilter(val) {
    try {
      localStorage.setItem(this.localStorageFilterKey, val);
    }
    catch (err) {
      console.log("Local storage not available.");
    }
  }

  getFilter() {
    try {
      let storedFilter = localStorage.getItem(this.localStorageFilterKey);
      if (storedFilter)
        return storedFilter;
    }
    catch (err) {
      console.log("Local storage not available.");
    }
    return null;
  } 
}
