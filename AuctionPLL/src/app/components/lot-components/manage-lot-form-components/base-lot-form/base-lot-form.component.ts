import { HttpEventType } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import {
    UntypedFormArray,
    UntypedFormBuilder,
    UntypedFormControl,
    UntypedFormGroup,
    Validators
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { ImagesViewModel } from 'src/app/generated-models/lot-models/images-view-model';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';
import { ImagesService } from 'src/app/services/images.service';
import { LotService } from 'src/app/services/lot.service';
import { CarBrandArray, CarBrandsMapping } from 'src/app/utils/car-brand-util';

@Component({
    template: ''
})
export abstract class BaseLotFormComponent {

    public get imageArray() {
        return this.lotForm.get('images') as UntypedFormArray;
    }
    public get CarBrands() {
        return CarBrandArray;
    }

    public saved: boolean = false;
    public CarBrandMapping = CarBrandsMapping;
    public lotForm: UntypedFormGroup;
    @ViewChild('file') public fileInput: any;
    constructor(
        protected lotService: LotService,
        public formBuilder: UntypedFormBuilder,
        protected toastrService: ToastrService,
        protected imagesService: ImagesService
    ) { }

    public initForm(lot: LotViewModel = null) {
        this.lotForm = this.formBuilder.group({
            nameLot: [lot?.nameLot ?? null, [
                Validators.required,
                Validators.minLength(5),
                Validators.maxLength(35)
            ]],
            description: [lot?.description ?? null, [
                Validators.required,
                Validators.minLength(10),
                Validators.maxLength(70)
            ]],
            startPrice: [lot?.startPrice ?? null, [
                Validators.required,
                Validators.pattern('^(0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*)$'),
                Validators.min(1)
            ]],
            currentPrice: [lot?.currentPrice ?? null],
            year: [lot?.year ?? null, [
                Validators.required,
                Validators.pattern('[0-9]{4}'),
                Validators.min(1806)
            ]],
            carBrand: [lot?.carBrand ?? null, Validators.required],
            image: [lot?.image ?? null, Validators.required],
            images: lot?.images
                ? this.formBuilder.array(this.initImages(lot?.images))
                : this.formBuilder.array([]),
        });
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

    public uploadMainImage(files: any) {
        if (files.length === 0)
            return;

        this.imagesService.uploadMainImage(files)
            .subscribe((event) => {
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

    private initImages(images: ImagesViewModel): any[] {
        const array = [];
        for (let i = 0; i < this.lotService.numbersOfImages; i++) {
            array.push(images['image' + (i + 1)]);
        }
        return array;
    }

}