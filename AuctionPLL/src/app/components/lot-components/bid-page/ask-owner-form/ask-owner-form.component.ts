import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AskOwner } from 'src/app/models/ask-owner';
import { AuthService } from 'src/app/services/auth.service';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-ask-owner-form',
  templateUrl: './ask-owner-form.component.html',
  styleUrls: ['./ask-owner-form.component.less']
})
export class AskOwnerFormComponent implements OnInit {

  constructor(
    private toastrService: ToastrService,
    private authService: AuthService,
    private lotService: LotService,
    private activeModal: NgbActiveModal
  ) {
    this.myForm = new UntypedFormGroup({
      ownerEmail: new UntypedFormControl(''),
      text: new UntypedFormControl('', [Validators.required]),
      fullName: new UntypedFormControl('', [Validators.required, Validators.minLength(2)]),
      userEmail: new UntypedFormControl('')
    });
  }

  public myForm: UntypedFormGroup;
  public ngOnInit() { }

  @Input() ownerEmail: string;
  public onSubmit() {
    if (!this.myForm.invalid) {
      const askOwner: AskOwner = {
        ownerEmail: this.ownerEmail,
        text: this.myForm.controls['text'].value,
        fullName: this.myForm.controls['fullName'].value,
        userEmail: this.authService.getUserEmailFromToken()
      };
      this.lotService.askOwner(askOwner)
        .subscribe((_) => {
          this.close();
          this.toastrService.success('Message is sended to owner');
          this.myForm.reset();
        }, (_) => {
          this.toastrService.error('some data is incorect');
        });
    }
  }

  public close() {
    this.activeModal.close();
  }

}
