import { Injectable } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { from, Observable } from 'rxjs';
import { ConfirmationDialogComponent } from './confirmation-dialog.component';

@Injectable()
export class ConfirmationDialogService {

    modalOptions: NgbModalOptions;
    constructor(private modalService: NgbModal) { }

    public confirm(
        title: string,
        message: string,
        btnOkText: string = 'OK',
        btnCancelText: string = 'Cancel'): Observable<boolean> {
        this.modalOptions = {
            backdrop: 'static',
            backdropClass: 'customBackdrop'
        };
        const modalRef = this.modalService.open(ConfirmationDialogComponent, this.modalOptions);
        modalRef.componentInstance.title = title;
        modalRef.componentInstance.message = message;
        modalRef.componentInstance.btnOkText = btnOkText;
        modalRef.componentInstance.btnCancelText = btnCancelText;

        return from(modalRef.result);
    }

}
