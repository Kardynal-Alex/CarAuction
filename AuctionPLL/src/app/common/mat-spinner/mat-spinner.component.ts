import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-mat-spinner',
  templateUrl: './mat-spinner.component.html',
  styleUrls: ['./mat-spinner.component.less']
})
export class MatSpinnerComponent {
  @Input() diameter: number = 600;
  @Input() strokeWidth: number = 50;
  constructor() { }
}
