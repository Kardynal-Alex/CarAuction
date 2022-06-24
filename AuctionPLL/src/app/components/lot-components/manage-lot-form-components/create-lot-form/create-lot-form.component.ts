import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ComponentCanDeactivate } from 'src/app/guards/exit.about.guard';
import { Images } from 'src/app/models/lot-models/images';
import { Lot } from 'src/app/models/lot-models/lot';
import { AuthService } from 'src/app/services/auth.service';
import { ImagesService } from 'src/app/services/images.service';
import { LotService } from 'src/app/services/lot.service';
import { BaseLotFormComponent } from '../base-lot-form/base-lot-form.component';

@Component({
  selector: 'app-create-lot-form',
  templateUrl: './create-lot-form.component.html',
  styleUrls: ['./create-lot-form.component.less']
})
export class CreateLotFormComponent extends BaseLotFormComponent implements OnInit, ComponentCanDeactivate {
  constructor(
    protected toastrService: ToastrService,
    protected lotService: LotService,
    private router: Router,
    public formBuilder: UntypedFormBuilder,
    private authService: AuthService,
    protected imagesService: ImagesService
  ) {
    super(lotService, formBuilder, toastrService, imagesService);
  }

  public ngOnInit(): void {
    this.initForm();
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
      carBrand: this.lotForm.controls.carBrand.value,
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
      .subscribe((_) => {
        this.toastrService.success('Successfully added!');
        this.saved = true;
        this.router.navigate(['/cabinet/userlots']);
      }, (_) => {
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
}
