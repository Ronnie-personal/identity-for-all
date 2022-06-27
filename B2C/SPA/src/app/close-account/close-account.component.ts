import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { BlobService } from '../blob.service';
import { MyBlob } from '../blob';
import { MyProfile, ResponseMessage } from '../profile';
import { msalConfig } from '../auth-config';

@Component({
  selector: 'app-close-account',
  templateUrl: './close-account.component.html',
  styleUrls: ['./close-account.component.css']
})
export class CloseAccountComponent implements OnInit {
  oid: string = null;
  blobs: MyBlob[] = [];
  myProfile: MyProfile = null;
  resMessage: ResponseMessage = null;
  constructor(private service: AccountService, private authService: MsalService, private blobService: BlobService) { }

  ngOnInit(): void {
    this.oid = this.authService.instance.getActiveAccount()?.idTokenClaims.sub;
    this.deleteAccount(this.oid);
  }

  deleteAccount(id: string): void {
    if (id != null) {
      this.service.deleteMyProfile(id).subscribe((res: ResponseMessage) => {
        this.resMessage = res;
        this.authService.logoutRedirect({
          postLogoutRedirectUri: msalConfig.auth.redirectUri
        })
      });
    }
  }

  getAccount(id: string): void
  {
      this.service.getMyProfile(id)
      .subscribe((myProfile: MyProfile) => {
        this.myProfile = myProfile;
      });      
  }
}
