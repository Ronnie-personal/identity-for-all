import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyBlob } from './blob';

@Injectable({
  providedIn: 'root'
})
export class BlobService {
  url = "https://localhost:7249/MyBlobFile";

  constructor(private http: HttpClient) { }

  getBlobs() { 
    return this.http.get<MyBlob[]>(this.url);
  }

  postBlob(blob: MyBlob) { 
    return this.http.post(this.url, blob, {observe: 'response', responseType: 'blob'});
  }
}
