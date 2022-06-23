import { trigger, state, style, transition, animate } from '@angular/animations';
import { HttpEventType } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import {
  UntypedFormArray,
  UntypedFormBuilder,
  UntypedFormControl,
  UntypedFormGroup,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { AuthorDescription } from 'src/app/models/author-description';
import { CarBrandArray, CarBrands } from 'src/app/models/lot-models/car-brand';
import { Images } from 'src/app/models/lot-models/images';
import { Lot } from 'src/app/models/lot-models/lot';
import { AuthService } from 'src/app/services/auth.service';
import { AuthorDescriptionService } from 'src/app/services/author-description.service';
import { ImagesService } from 'src/app/services/images.service';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-update-lot-form',
  templateUrl: './update-lot-form.component.html',
  styleUrls: ['./update-lot-form.component.less'],
  animations: [
    trigger('toggle', [
      state('hide', style({
        height: '0px',
        overflow: 'hidden'
      })),
      state('show', style({
        height: '*'
      })),
      transition('hide => show', animate('0.35s ease')),
      transition('show => hide', animate('0.35s ease'))
    ])
  ]
})
export class UpdateLotFormComponent implements OnInit {
  public get imageArray() {
    return this.lotForm.get('images') as UntypedFormArray;
  }

  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private activateRoute: ActivatedRoute,
    public formBuilder: UntypedFormBuilder,
    public formBuilder1: UntypedFormBuilder,
    private router: Router,
    private imagesService: ImagesService,
    private authService: AuthService,
    private authorDescriptionService: AuthorDescriptionService
  ) { }

  public CarBrandMapping = CarBrands;
  public get CarBrands() {
    return CarBrandArray;
  }

  public isCollapsedForm = false;
  public isCollapsedAuthorOpinion = !false;
  public saved: boolean = false;
  public lot: Lot;
  public routeId: number;
  ngOnInit(): void {
    this.routeId = this.activateRoute.snapshot.params['id'];
    this.lotService.getLotById(this.routeId)
      .subscribe((_) => {
        this.lot = _;
        this.initForm(_);
      });
    this.authorDescriptionService.getAuthorDescriptionByLotId(this.routeId)
      .subscribe((_) => {
        this.initAuthorDescriptionForm(_);
      });
  }

  public lotForm: UntypedFormGroup;
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
      carBrand: [lot.carBrand, Validators.required],
      image: [lot.image, Validators.required],
      images: this.formBuilder.array(this.initImages(lot.images))
    });
  }

  public authorDescription: AuthorDescription;
  public authorForm: UntypedFormGroup;
  public isNewDescription: boolean = false;
  private initAuthorDescriptionForm(authorDescription: AuthorDescription) {
    this.authorDescription = authorDescription;
    const description = !!authorDescription?.description ? authorDescription?.description : null;
    this.isNewDescription = !description;
    this.authorForm = this.formBuilder1.group({
      description: [description, [
        Validators.required,
        Validators.minLength(20)
      ]]
    });
  }

  public createAuthorDescription() {
    const description = this.authorForm.controls.description.value;
    if (this.isNewDescription) {
      this.authorDescription = {
        id: 0,
        userId: this.authService.getUserIdFromToken(),
        lotId: parseInt(this.routeId.toString()),
        description: description
      };
      this.authorDescriptionService.addAuthorDescription(this.authorDescription)
        .subscribe(_ => {
          this.toastrService.success('Author Description is created');
          this.saved = true;
          this.router.navigate(['userlots'])
        }, (_) => this.toastrService.error(ErrorMessages.Error))
    } else {
      this.authorDescriptionService.updateAuthorDescription({ ...this.authorDescription, description })
        .subscribe(_ => {
          this.toastrService.success('Author Description is updated');
          this.saved = true;
          this.router.navigate(['userlots'])
        }, (_) => this.toastrService.error(ErrorMessages.Error))
    }
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
      this.imageArray.push(new UntypedFormControl(null, [Validators.required]));
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
      .subscribe((_) => {
        this.toastrService.success('Lot is updated');
        this.saved = true;
        this.router.navigate(['userlots']);
      }, (_) => this.toastrService.error(ErrorMessages.Error));
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
      return confirm('Are you want to leave the page?');
    } else {
      return true;
    }
  }

  public toggleForm() {
    this.isCollapsedForm = !this.isCollapsedForm;
  }

  public toggleAuthorOpinion() {
    this.isCollapsedAuthorOpinion = !this.isCollapsedAuthorOpinion;
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
      .subscribe((event) => {
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
        .subscribe((_) => {
          const form = this.imageArray.at(index);
          form.patchValue(null);
          this.toastrService.success('Photo is deleted');
        });
    }
  }

  public deleteMainPhoto() {
    if (!!this.lotForm.controls.image.value) {
      this.lotService.deletePhoto(this.lotForm.controls.image.value)
        .subscribe((_) => {
          this.lotForm.controls.image.patchValue(null);
          this.toastrService.success('Photo is deleted');
        });
    }
  }

}
