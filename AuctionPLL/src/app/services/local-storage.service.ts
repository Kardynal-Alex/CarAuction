import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  private localStorage: Storage;

  constructor() {
    this.localStorage = window.localStorage;
  }

  public get(key: string): string {
    return this.localStorage.getItem(key);
  }

  public set(key: string, value: any): void {
    this.localStorage.setItem(key, value);
  }

  public remove(key: string): void {
    this.localStorage.removeItem(key);
  }

  public clear(): void {
    this.localStorage.clear();
  }
}