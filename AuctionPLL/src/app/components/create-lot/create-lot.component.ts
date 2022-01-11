import { Component, OnInit,ViewChild} from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { Images } from 'src/app/models/images';
import { ComponentCanDeactivate } from 'src/app/guards/exit.about.guard';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-create-lot',
  templateUrl: './create-lot.component.html',
  styleUrls: ['./create-lot.component.css']
})

export class CreateLotComponent implements OnInit, ComponentCanDeactivate {
  constructor(private toastrService:ToastrService,
              private lotService:LotService,
              private httpClient:HttpClient,
              private localStorage: LocalStorageService,
              private router:Router) { }
  
  saved: boolean = false;
  ngOnInit(): void {
    this.imagePath='';
    this.photoIsEmpty=true;
    this.images={
      Image1:'',Image2:'',Image3:'',Image4:'',Image5:'',Image6:'',Image7:'',Image8:'',Image9:'',Id:0
    };
    this.numbers=Array.from(Array(this.lotService.numbersOfImages).keys());
    this.checkIfAllImagesIsUploaded();
  }
  getUserId():string{
    var payload=JSON.parse(window.atob(this.localStorage.get('token').split('.')[1]));
    return payload.id;
  } 
  
  images:Images;

  photoIsEmpty:boolean;
  deleteMainPhoto(){
    if(this.imagePath!==''){
      this.photoIsEmpty=true;
      this.lotService.deletePhoto(this.imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.imagePath='';
      });
    }
  }

  checkIfAllImagesIsUploaded():boolean{
    for(let i=0;i<this.numbers.length;i++){
      if(this.images['image'+(i+1)]=='' || this.images['image'+(i+1)]==undefined){
        return false;
      }
    }
    return true;
  }
  response;
  createLot(form:NgForm){
    if(this.photoIsEmpty==false && this.checkIfAllImagesIsUploaded()==true){
      const userid=this.getUserId()
      const lot:Lot={
        Id:0,
        NameLot:form.value.NameLot,
        StartPrice:form.value.StartPrice,
        IsSold:false,
        Image:this.imagePath,
        Description:form.value.Description,
        UserId:userid,
        StartDateTime: new Date(Date.now()), 
        CurrentPrice:form.value.StartPrice,
        Year:form.value.Year,
        User:null,
        LotState:{
          Id:0,
          OwnerId:userid,
          FutureOwnerId:userid,
          CountBid:0,
          LotId:0
        },
        Images:this.images
      }
      this.lotService.createLot(lot).subscribe(response=>{
        this.toastrService.success('Successfully added!');
        this.saved = true;
        this.router.navigate(['/userlots']);
      },
      error=>{
        if(this.imagePath.length!==0){
          this.toastrService.error('Something went wrong!');
        }});
    }else{
      this.toastrService.error("Image is not uploaded!");
    }
  }

  canDeactivate() : boolean | Observable<boolean>{
    if(!this.saved){
        return confirm("Are you want to leave the page?");
    }
    else{
        return true;
    }
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }

  @ViewChild('file') fileInput: any;
  imagePath:string;
  uploadFile(files){
    if(files.length === 0)
        return;
    let uploadApiPhoto='https://localhost:44325/api/upload';
    let fileToUpload=<File>files[0];
    let formData=new FormData();
    formData.append('file',fileToUpload,fileToUpload.name);
    this.httpClient.post(uploadApiPhoto,formData, {reportProgress: true, observe: 'events'}).
    subscribe(event=>
    {
      if (event.type === HttpEventType.Response) {
             this.response=event.body; 
             this.photoIsEmpty=false;
             this.imagePath=this.response['dbPath'];
             this.toastrService.success('Photo is uploaded!');
             this.fileInput.nativeElement.value = '';
      }
    });
  }

  uploadFiles(files, field, number){
    if(files.length === 0)
      return;
    let uploadApiPhoto='https://localhost:44325/api/upload';
    let fileToUpload=<File>files[0];
    let formData=new FormData();
    formData.append('file',fileToUpload,fileToUpload.name);
    this.httpClient.post(uploadApiPhoto,formData, {reportProgress: true, observe: 'events'}).
    subscribe(event=>
    {
      if (event.type === HttpEventType.Response) {
            this.response=event.body;
            document.getElementById('but-'+number).style.display='none';
            this.images[field]=this.response['dbPath'];
            this.toastrService.success('Photo is uploaded!');
      }
    });
  }

  numbers=[];
  deletePhotoByPath(imagePath:string, field:string){
    if(imagePath!==''){
      this.lotService.deletePhoto(imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.images[field]='';
      });
    }
  }
}
