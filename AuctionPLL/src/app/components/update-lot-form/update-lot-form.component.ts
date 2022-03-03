import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BaseUrl } from 'src/app/common/constants/urls';
import { Images } from 'src/app/models/images';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-update-lot-form',
  templateUrl: './update-lot-form.component.html',
  styleUrls: ['./update-lot-form.component.less']
})
export class UpdateLotFormComponent implements OnInit {
  public get imageArray() {
    return this.lotForm.get('images') as FormArray;
  }

  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private httpClient: HttpClient,
    private activateRoute: ActivatedRoute,
    public formBuilder: FormBuilder,
    private router: Router
  ) { }

  public saved: boolean = false;
  public lot: Lot;
  public routeId: number;
  ngOnInit(): void {
    this.routeId = this.activateRoute.snapshot.params['id'];
    this.lotService.getLotById(this.routeId)
      .subscribe(_ => {
        this.lot = _;
        this.initForm(_);
      });
  }

  public lotForm: FormGroup;
  private initForm(lot: Lot) {
    this.lotForm = this.formBuilder.group({
      nameLot: [lot.nameLot, [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(35)
      ]],
      description: [lot.description, [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(70)
      ]],
      startPrice: [lot.startPrice],
      currentPrice: [lot.currentPrice],
      year: [lot.year, [
        Validators.required,
        Validators.pattern('[0-9]{4}'),
        Validators.min(1806)
      ]],
      image: [lot.image, Validators.required],
      images: this.formBuilder.array(this.initImages(lot.images))
    });
  }

  private initImages(images: Images): any[] {
    const array = [];
    for (let i = 0; i < this.lotService.numbersOfImages; i++) {
      array.push(images['image' + (i + 1)]);
    }
    return array;
  }

  public addImage() {
    if (this.lotService.numbersOfImages !== this.imageArray.controls.length) {
      this.imageArray.push(new FormControl(null, [Validators.required]));
    }
  }

  public removeImage(index: number) {
    this.imageArray.removeAt(index);
    this.lotForm.markAsDirty();
  }

  public updateLot() {
    this.lot = {
      ...this.lot,
      ... this.lotForm.value,
      user: null,
      lotState: null,
      images: this.getImages(this.lot)
    };
    this.lotService.updateLot(this.lot)
      .subscribe(_ => {
        this.toastrService.success("Lot is updated");
        this.saved = true;
        this.router.navigate(['userlots'])
      }, _ => this.toastrService.error("Error!"));
  }

  private getImages(lot: Lot): Images {
    let images: Images = new Images();

    images.id = this.lot.id;
    let index = 0;
    for (let image of this.lotForm.controls.images.value) {
      images['image' + (index + 1)] = image;
      index++;
    }

    return images;
  }

  public canDeactivate(): boolean | Observable<boolean> {
    if (!this.saved) {
      return confirm("Are you want to leave the page?");
    } else {
      return true;
    }
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }


  @ViewChild('file') fileInput: any;
  public uploadMainImage(files: any) {
    if (files.length === 0)
      return;
    let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
    let fileToUpload = <File>files[0];
    let formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          const response = event.body;
          this.lotForm.controls.image.patchValue(response['dbPath']);
          this.toastrService.success('Photo is uploaded!');
          this.fileInput.nativeElement.value = '';
        }
      });
  }

  public uploadImages(files: any, index: number) {
    if (files.length === 0)
      return;
    let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
    let fileToUpload = files.target.files[0];
    let formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          const response = event.body;
          const myForm = this.imageArray.at(index);
          myForm.patchValue(response['dbPath']);
          this.toastrService.success('Photo is uploaded!');
        }
      });
  }

  public deletePhotoByPath(imagePath: string, index: number) {
    if (!!imagePath) {
      this.lotService.deletePhoto(imagePath)
        .subscribe(_ => {
          const form = this.imageArray.at(index);
          form.patchValue(null);
          this.toastrService.success("Photo is deleted");
        });
    }
  }

  public deleteMainPhoto() {
    if (!!this.lotForm.controls.image.value) {
      this.lotService.deletePhoto(this.lotForm.controls.image.value)
        .subscribe(_ => {
          this.lotForm.controls.image.patchValue(null);
          this.toastrService.success("Photo is deleted");
        });
    }
  }

}
