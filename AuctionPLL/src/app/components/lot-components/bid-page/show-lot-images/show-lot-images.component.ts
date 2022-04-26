import { Component, OnInit, Output } from '@angular/core';
import { Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Lot } from 'src/app/models/lot-models/lot';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-show-lot-images',
  templateUrl: './show-lot-images.component.html',
  styleUrls: ['./show-lot-images.component.less']
})
export class ShowLotImagesComponent implements OnInit {

  constructor(
    private lotService: LotService,
    private activeModal: NgbActiveModal
  ) { }

  public ngOnInit() {
    this.numbers = Array.from(Array(this.lotService.numbersOfImages).keys());
  }
  public numbers = [];
  @Input() lot: Lot;
  public close() {
    this.activeModal.close();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }
}
