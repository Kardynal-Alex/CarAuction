import { trigger, state, style, transition, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { AuthorDescription } from 'src/app/models/author-description';
import { Images } from 'src/app/models/lot-models/images';
import { Lot } from 'src/app/models/lot-models/lot';
import { AuthService } from 'src/app/services/auth.service';
import { AuthorDescriptionService } from 'src/app/services/author-description.service';
import { ImagesService } from 'src/app/services/images.service';
import { LotService } from 'src/app/services/lot.service';
import { BaseLotFormComponent } from '../base-lot-form/base-lot-form.component';

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
export class UpdateLotFormComponent extends BaseLotFormComponent implements OnInit {

  public isCollapsedForm = false;
  public isCollapsedAuthorOpinion = !false;
  public lot: Lot;
  public routeId: number;
  public authorDescription: AuthorDescription;
  public authorForm: UntypedFormGroup;
  public isNewDescription: boolean = false;
  constructor(
    protected toastrService: ToastrService,
    protected lotService: LotService,
    private activateRoute: ActivatedRoute,
    public formBuilder: UntypedFormBuilder,
    public formBuilder1: UntypedFormBuilder,
    private router: Router,
    protected imagesService: ImagesService,
    private authService: AuthService,
    private authorDescriptionService: AuthorDescriptionService
  ) {
    super(lotService, formBuilder, toastrService, imagesService);
  }

  public ngOnInit(): void {
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
        .subscribe((_) => {
          this.toastrService.success('Author Description is updated');
          this.saved = true;
          this.router.navigate(['userlots'])
        }, (_) => this.toastrService.error(ErrorMessages.Error))
    }
  }

  public updateLot() {
    this.lot = {
      ...this.lot,
      ... this.lotForm.value,
      user: null,
      lotState: null,
      images: this.getImages()
    };
    this.lotService.updateLot(this.lot)
      .subscribe((_) => {
        this.toastrService.success('Lot is updated');
        this.saved = true;
        this.router.navigate(['userlots']);
      }, (_) => this.toastrService.error(ErrorMessages.Error));
  }

  private getImages(): Images {
    let images: Images = new Images();

    images.id = this.lot.id;
    let index = 0;
    for (let image of this.lotForm.controls.images.value) {
      images['image' + (index + 1)] = image;
      index++;
    }

    return images;
  }

  public toggleForm() {
    this.isCollapsedForm = !this.isCollapsedForm;
  }

  public toggleAuthorOpinion() {
    this.isCollapsedAuthorOpinion = !this.isCollapsedAuthorOpinion;
  }

}
