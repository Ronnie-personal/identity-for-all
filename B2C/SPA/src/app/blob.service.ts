import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyBlob } from './blob';
import { protectedResources } from './auth-config';

@Injectable({
  providedIn: 'root'
})
export class BlobService {
  url = protectedResources.MyStorageApi.endpoint;
  constructor(private http: HttpClient) { }

  getBlobs() { 
    return this.http.get<MyBlob[]>(this.url);
  }

  postBlob(blob: MyBlob) { 
    return this.http.post(this.url, blob, {observe: 'response', responseType: 'blob'});
  }
}
