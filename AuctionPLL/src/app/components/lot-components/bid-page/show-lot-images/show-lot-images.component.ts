import { Component, OnInit, Output } from '@angular/core';
import { Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-show-lot-images',
  templateUrl: './show-lot-images.component.html',
  styleUrls: ['./show-lot-images.component.less']
})
export class ShowLotImagesComponent implements OnInit {

  public numbers = [];
  @Input() public lot: LotViewModel;
  constructor(
    private lotService: LotService,
    private activeModal: NgbActiveModal
  ) { }

  public ngOnInit() {
    this.numbers = Array.from(Array(this.lotService.numbersOfImages).keys());
  }

  public close() {
    this.activeModal.close();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }
}
