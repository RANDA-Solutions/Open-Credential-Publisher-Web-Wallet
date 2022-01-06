import { Injectable } from '@angular/core';
import { AbstractSecurityStorage, LoggerService, OpenIdConfiguration } from 'angular-auth-oidc-client';

@Injectable({ providedIn: 'root' })
export class AuthStorageService {
  constructor(private loggerService: LoggerService) {}

  read(key: string, configuration: OpenIdConfiguration): any {
    const { configId } = configuration;

    if (!this.hasStorage()) {
      this.loggerService.logDebug(configId, `Wanted to read '${key}' but Storage was undefined`);

      return null;
    }

    const storage = this.getStorage(configuration);

    if (!storage) {
      this.loggerService.logDebug(configId, `Wanted to read config for '${configId}' but Storage was falsy`);

      return null;
    }

    const storedConfig = storage.read(configId);

    if (!storedConfig) {
      return null;
    }

    let config = JSON.parse(storedConfig);
	return config[key];
  }

  write(key: string, value: any, configuration: OpenIdConfiguration): boolean {
    const { configId } = configuration;

    if (!this.hasStorage()) {
      this.loggerService.logDebug(configId, `Wanted to write '${value}' but Storage was falsy`);

      return false;
    }

    const storage = this.getStorage(configuration);
    if (!storage) {
      this.loggerService.logDebug(configId, `Wanted to write '${value}' but Storage was falsy`);

      return false;
    }

    value = value || null;

	const storedConfig = storage.read(configId);
	if(!storedConfig) {
		this.loggerService.logDebug(configId, `Wanted to write '${value}' but missing config`);

		return false;
	}
	let jsonConfig = JSON.parse(storedConfig);
	jsonConfig[key] = value;

    storage.write(configId, JSON.stringify(jsonConfig));

    return true;
  }

  remove(key: string, configuration: OpenIdConfiguration): boolean {
    if (!this.hasStorage()) {
      this.loggerService.logDebug(configuration.configId, `Wanted to remove '${key}' but Storage was falsy`);

      return false;
    }

    const storage = this.getStorage(configuration);
    if (!storage) {
      this.loggerService.logDebug(configuration.configId, `Wanted to write '${key}' but Storage was falsy`);

      return false;
    }

    storage.remove(key);

    return true;
  }

  // TODO THIS STORAGE WANTS AN ID BUT CLEARS EVERYTHING
  clear(configuration: OpenIdConfiguration): boolean {
    if (!this.hasStorage()) {
      this.loggerService.logDebug(configuration.configId, `Wanted to clear storage but Storage was falsy`);

      return false;
    }

    const storage = this.getStorage(configuration);
    if (!storage) {
      this.loggerService.logDebug(configuration.configId, `Wanted to clear storage but Storage was falsy`);

      return false;
    }

    storage.clear();

    return true;
  }

  private getStorage(configuration: OpenIdConfiguration): AbstractSecurityStorage {
    const { storage } = configuration || {};

    return storage;
  }

  private hasStorage(): boolean {
    return typeof Storage !== 'undefined';
  }
}