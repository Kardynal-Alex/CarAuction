import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AskOwner } from 'src/app/models/ask-owner';
import { AuthService } from 'src/app/services/auth.service';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-ask-owner-form',
  templateUrl: './ask-owner-form.component.html',
  styleUrls: ['./ask-owner-form.component.css']
})
export class AskOwnerFormComponent implements OnInit {

  constructor(private toastrService:ToastrService,
              private authService:AuthService,
              private lotService:LotService) 
              { 
                this.myForm=new FormGroup({
                  OwnerEmail:new FormControl(''),
                  Text:new FormControl('', [Validators.required]),
                  FullName:new FormControl('', [Validators.required,Validators.minLength(2)]),
                  UserEmail:new FormControl('')
                });
              }

  myForm:FormGroup;
  ngOnInit(){
  }

  @Input() ownerEmail:string;
  onSubmit(){
    if(!this.myForm.invalid){
      const askOwner:AskOwner={
        OwnerEmail:this.ownerEmail,
        Text:this.myForm.controls['Text'].value,
        FullName:this.myForm.controls['FullName'].value,
        UserEmail:this.authService.getUserEmail()
      }
      this.lotService.askOwner(askOwner).subscribe(response=>{
        this.closeForm();
        this.toastrService.success('Message is sended to owner');
        this.myForm.reset();
      }, error=>{
        this.toastrService.error('some data is incorect');
      })
    }
  }

  openForm(){
    document.getElementById("myForm").style.display = "block";
  }

  closeForm() {
    document.getElementById("myForm").style.display = "none";
  }

}
