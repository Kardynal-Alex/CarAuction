import { HttpEventType } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { ComponentCanDeactivate } from 'src/app/guards/exit.about.guard';
import { Images } from 'src/app/models/lot-models/images';
import { Lot } from 'src/app/models/lot-models/lot';
import { AuthService } from 'src/app/services/auth.service';
import { ImagesService } from 'src/app/services/images.service';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-create-lot-form',
  templateUrl: './create-lot-form.component.html',
  styleUrls: ['./create-lot-form.component.less']
})
export class CreateLotFormComponent implements OnInit, ComponentCanDeactivate {
  public get imageArray() {
    return this.lotForm.get('images') as FormArray;
  }

  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private router: Router,
    public formBuilder: FormBuilder,
    private authService: AuthService,
    private imagesService: ImagesService
  ) { }

  private saved: boolean = false;
  public ngOnInit(): void {
    this.initForm();
  }

  public lotForm: FormGroup;
  private initForm() {
    this.lotForm = this.formBuilder.group({
      nameLot: [null, [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(35)
      ]],
      description: [null, [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(70)
      ]],
      startPrice: [null, [
        Validators.required,
        Validators.pattern('^(0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*)$'),
        Validators.min(1)
      ]],
      year: [null, [
        Validators.required,
        Validators.pattern('[0-9]{4}'),
        Validators.min(1806)
      ]],
      image: [null, Validators.required],
      images: this.formBuilder.array([])
    });
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

  public createLot() {
    const userId = this.authService.getUserIdFromToken();
    const lot: Lot = {
      id: 0,
      nameLot: this.lotForm.controls.nameLot.value,
      startPrice: this.lotForm.controls.startPrice.value,
      isSold: false,
      image: this.lotForm.controls.image.value,
      description: this.lotForm.controls.description.value,
      userId: userId,
      startDateTime: new Date(Date.now()),
      currentPrice: this.lotForm.controls.startPrice.value,
      year: this.lotForm.controls.year.value,
      user: null,
      lotState: {
        id: 0,
        ownerId: userId,
        futureOwnerId: userId,
        countBid: 0,
        lotId: 0
      },
      images: this.getImages()
    };
    this.lotService.createLot(lot)
      .subscribe(_ => {
        this.toastrService.success('Successfully added!');
        this.saved = true;
        this.router.navigate(['/userlots']);
      }, _ => {
        this.toastrService.error('Something went wrong!');
      });
  }

  private getImages(): Images {
    let images: Images = new Images();

    let index = 0;
    for (let image of this.lotForm.controls.images.value) {
      images['image' + (index + 1)] = image;
      index++;
    }

    return images;
  }

  public canDeactivate(): boolean | Observable<boolean> {
    if (!this.saved) {
      return confirm('Are you want to leave the page?');
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

    this.imagesService.uploadMainImage(files)
      .subscribe(event => {
        if (event.type === HttpEventType.Response) {
          const response = event.body;
          this.lotForm.controls.image.patchValue(response[CommonConstants.ImageResponsePath]);
          this.toastrService.success('Photo is uploaded!');
          this.fileInput.nativeElement.value = '';
        }
      });
  }

  public uploadImages(files: any, index: number) {
    if (files.length === 0)
      return;

    this.imagesService.uploadImages(files)
      .subscribe(event => {
        if (event.type === HttpEventType.Response) {
          const response = event.body;
          const myForm = this.imageArray.at(index);
          myForm.patchValue(response[CommonConstants.ImageResponsePath]);
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
          this.toastrService.success('Photo is deleted');
        });
    }
  }

  public deleteMainPhoto() {
    if (!!this.lotForm.controls.image.value) {
      this.lotService.deletePhoto(this.lotForm.controls.image.value)
        .subscribe(_ => {
          this.lotForm.controls.image.patchValue(null);
          this.toastrService.success('Photo is deleted');
        });
    }
  }

}
