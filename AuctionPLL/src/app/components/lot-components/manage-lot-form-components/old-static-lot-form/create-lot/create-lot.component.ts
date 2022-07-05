import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LotService } from 'src/app/services/lot.service';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { ComponentCanDeactivate } from 'src/app/guards/exit.about.guard';
import { Observable } from 'rxjs';
import { BaseUrl } from 'src/app/common/constants/urls';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { ImagesViewModel } from 'src/app/generated-models/lot-models/images-view-model';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';

/**
  * @deprecated old form - use create-lot-form with reactive forms
*/
@Component({
  selector: 'app-create-lot',
  templateUrl: './create-lot.component.html',
  styleUrls: ['./create-lot.component.less']
})
export class CreateLotComponent implements OnInit, ComponentCanDeactivate {
  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private httpClient: HttpClient,
    private localStorage: LocalStorageService,
    private router: Router
  ) { }

  public saved: boolean = false;
  public ngOnInit(): void {
    this.imagePath = '';
    this.photoIsEmpty = true;
    this.images = {
      image1: '', image2: '', image3: '', image4: '', image5: '', image6: '', image7: '', image8: '', image9: '', id: 0
    };
    this.numbers = Array.from(Array(this.lotService.numbersOfImages).keys());
    this.checkIfAllImagesIsUploaded();
  }
  public getUserId(): string {
    var payload = JSON.parse(window.atob(this.localStorage.get(CommonConstants.JWTToken).split('.')[1]));
    return payload.id;
  }

  public images: ImagesViewModel;

  public photoIsEmpty: boolean;
  public deleteMainPhoto() {
    if (this.imagePath !== '') {
      this.photoIsEmpty = true;
      this.lotService.deletePhoto(this.imagePath).subscribe(response => {
        this.toastrService.success('Photo is deleted');
        this.imagePath = '';
      });
    }
  }

  public checkIfAllImagesIsUploaded(): boolean {
    for (let i = 0; i < this.numbers.length; i++) {
      if (this.images['image' + (i + 1)] === '' || !this.images['image' + (i + 1)]) {
        return false;
      }
    }
    return true;
  }

  public response;
  public createLot(form: NgForm) {
    if (this.photoIsEmpty == false && this.checkIfAllImagesIsUploaded()) {
      const userid = this.getUserId();
      const lot: LotViewModel = {
        id: 0,
        nameLot: form.value.NameLot,
        startPrice: form.value.StartPrice,
        isSold: false,
        image: this.imagePath,
        description: form.value.Description,
        userId: userid,
        startDateTime: new Date(Date.now()),
        currentPrice: form.value.StartPrice,
        year: form.value.Year,
        user: null,
        carBrand: 0,
        lotState: {
          id: 0,
          ownerId: userid,
          futureOwnerId: userid,
          countBid: 0,
          lotId: 0
        },
        images: this.images
      };
      this.lotService.createLot(lot)
        .subscribe(_ => {
          this.toastrService.success('Successfully added!');
          this.saved = true;
          this.router.navigate(['/userlots']);
        }, _ => {
          if (this.imagePath.length !== 0) {
            this.toastrService.error('Something went wrong!');
          }
        });
    } else {
      this.toastrService.error('Image is not uploaded!');
    }
  }

  public canDeactivate(): boolean | Observable<boolean> {
    if (!this.saved) {
      return confirm('Are you want to leave the page?');
    }
    else {
      return true;
    }
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  @ViewChild('file') fileInput: any;
  public imagePath: string;
  public uploadFile(files) {
    if (files.length === 0)
      return;
    let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
    let fileToUpload = <File>files[0];
    let formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          this.response = event.body;
          this.photoIsEmpty = false;
          this.imagePath = this.response['dbPath'];
          this.toastrService.success('Photo is uploaded!');
          this.fileInput.nativeElement.value = '';
        }
      });
  }

  public uploadFiles(files, field, number) {
    if (files.length === 0)
      return;
    let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
    let fileToUpload = <File>files[0];
    let formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          this.response = event.body;
          document.getElementById('but-' + number).style.display = 'none';
          this.images[field] = this.response['dbPath'];
          this.toastrService.success('Photo is uploaded!');
        }
      });
  }

  public numbers = [];
  public deletePhotoByPath(imagePath: string, field: string) {
    if (!!imagePath) {
      this.lotService.deletePhoto(imagePath)
        .subscribe(_ => {
          this.toastrService.success('Photo is deleted');
          this.images[field] = '';
        });
    }
  }
}
