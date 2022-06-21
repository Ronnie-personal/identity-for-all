import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { BlobService } from './../blob.service';
import { MyBlob } from '../blob';

@Component({
  selector: 'app-blob-view',
  templateUrl: './blob-view.component.html',
  styleUrls: ['./blob-view.component.css']
})
export class BlobViewComponent implements OnInit {
  
  blob?: MyBlob;

  blobs: MyBlob[] = [];

  displayedColumns = ['blobFile', 'lastModified', 'download'];

  constructor(private service: BlobService) { }

  ngOnInit(): void {
    this.getBlobs();
  }

  getBlobs(): void {
    this.service.getBlobs()
      .subscribe((blobs: MyBlob[]) => {
        this.blobs = blobs;
      });
  }

  postBlob(blob: MyBlob): void {
    this.service.postBlob(blob).subscribe(response =>
    {
      let fileName = response.headers.get('Content-Disposition')
        .split(';')[1].split('=')[1];
      //console.log(response.headers.keys());
      let returnBlob: Blob = response.body as Blob;
      let mydoc = document.createElement('a');
      mydoc.download = fileName;
      mydoc.href = window.URL.createObjectURL(returnBlob);
      mydoc.click()
    })
  }
}
