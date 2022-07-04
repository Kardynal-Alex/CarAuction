import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-view',
  templateUrl: './empty-view.component.html',
  styleUrls: ['./empty-view.component.less']
})
export class EmptyViewComponent {

  @Input() text = 'List is empty';
  constructor() { }

}
