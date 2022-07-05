import { Component, Input } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AskOwnerViewModel } from 'src/app/generated-models/lot-models/ask-owner-view-model';
import { AuthService } from 'src/app/services/auth.service';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-ask-owner-form',
  templateUrl: './ask-owner-form.component.html',
  styleUrls: ['./ask-owner-form.component.less']
})
export class AskOwnerFormComponent {

  @Input() public ownerEmail: string;
  public myForm: UntypedFormGroup;
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

  public onSubmit() {
    if (!this.myForm.invalid) {
      const askOwner: AskOwnerViewModel = {
        ownerEmail: this.ownerEmail,
        text: this.myForm.controls.text.value,
        fullName: this.myForm.controls.fullName.value,
        userEmail: this.authService.getUserEmailFromToken()
      };
      this.lotService.askOwner(askOwner)
        .subscribe((_) => {
          this.toastrService.success('Message is sended to owner');
          this.myForm.reset();
          this.close();
        }, (_) => {
          this.toastrService.error('some data is incorect');
        });
    }
  }

  public close() {
    this.activeModal.close();
  }

}
