import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { LotService } from 'src/app/services/lot.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { Lot } from 'src/app/models/lot-models/lot';
import { ComponentCanDeactivate } from 'src/app/guards/exit.about.guard';
import { Observable } from 'rxjs';
import { BaseUrl } from 'src/app/common/constants/urls';

/**
  * @deprecated old form - use update-lot-form with reactive forms
*/
@Component({
  selector: 'app-update-lot',
  templateUrl: './update-lot.component.html',
  styleUrls: ['./update-lot.component.less']
})
export class UpdateLotComponent implements OnInit, ComponentCanDeactivate {
  public lot: Lot;
  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private httpClient: HttpClient,
    private activateRoute: ActivatedRoute,
    private router: Router) { }

  public saved: boolean = false;
  public id: number;
  public getLot() {
    this.id = this.activateRoute.snapshot.params['id'];
    this.lotService.getLotById(this.id)
      .subscribe(resp => {
        this.lot = resp;
        this.imagePath = this.lot.image;
      });
  }

  public responseLot;
  public ngOnInit() {
    this.id = this.activateRoute.snapshot.params['id']
    this.lotService.getLotById(this.id)
      .subscribe(resp => {
        this.lot = resp;
        this.imagePath = this.lot.image;
      });
    this.numbers = Array.from(Array(this.lotService.numbersOfImages).keys());
  }

  public checkIfAllImagesIsUploaded(): boolean {
    for (let i = 0; i < this.numbers.length; i++) {
      if (this.lot['images']['image' + (i + 1)] == '' || !!this.lot['images']['image' + (i + 1)]) {
        return false;
      }
    }
    return true;
  }

  public updateLot() {
    if (this.imagePath && this.checkIfAllImagesIsUploaded()) {
      this.lot.image = this.imagePath;
      this.lot.user = null;
      this.lot.lotState = null;
      this.lotService.updateLot(this.lot)
        .subscribe(_ => {
          this.toastrService.success('Lot is updated');
          this.saved = true;
          this.router.navigate(['userlots'])
        }, _ => this.toastrService.error('Error!'));
    } else {
      this.toastrService.error('Download image!');
    }
  }

  public canDeactivate(): boolean | Observable<boolean> {
    if (!this.saved) {
      return confirm('Are you want to leave the page?');
    } else {
      return true;
    }
  }

  public response;
  public imagePath: string;
  @ViewChild('file') fileInput: any;
  uploadFile(files) {
    if (files.length === 0)
      return;
    let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          this.response = event.body;
          this.imagePath = (this.response['dbPath']);
          this.toastrService.success('Photo is uploaded!');
          this.fileInput.nativeElement.value = '';
        }
      });
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  public deleteMainPhoto() {
    if (!!this.imagePath) {
      this.lotService.deletePhoto(this.imagePath)
        .subscribe(_ => {
          this.toastrService.success('Photo is deleted');
          this.imagePath = '';
        });
    }
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
          this.lot['images'][field] = this.response['dbPath'];
          this.toastrService.success('Photo is uploaded!');
        }
      });
  }

  public numbers = [];
  public deletePhotoByPath(imagePath: string, number: number) {
    if (imagePath !== '') {
      this.lotService.deletePhoto(imagePath)
        .subscribe(_ => {
          this.toastrService.success('Photo is deleted');
          this.lot['images']['image' + number] = '';
          document.getElementById('but-' + number).style.display = 'block';
        });
    }
  }
}
