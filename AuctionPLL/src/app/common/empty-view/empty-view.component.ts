import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-empty-view',
  templateUrl: './empty-view.component.html',
  styleUrls: ['./empty-view.component.less']
})
export class EmptyViewComponent implements OnInit {

  @Input() text = 'List is empty';
  constructor() { }

  ngOnInit(): void {
  }

}
