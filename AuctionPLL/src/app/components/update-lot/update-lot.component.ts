import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { LotService } from 'src/app/services/lot.service';
import { ToastrService } from 'ngx-toastr';
import { Router,ActivatedRoute } from '@angular/router';
import { Lot } from 'src/app/models/lot';
import { ComponentCanDeactivate } from 'src/app/guards/exit.about.guard';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-update-lot',
  templateUrl: './update-lot.component.html',
  styleUrls: ['./update-lot.component.css']
})
export class UpdateLotComponent implements OnInit, ComponentCanDeactivate {
  lot:Lot;
  constructor(private toastrService:ToastrService,
              private lotService:LotService,
              private httpClient:HttpClient,
              private activateRoute:ActivatedRoute,
              private router:Router) { }

  saved: boolean = false;
  id:number;
  getLot(){
    this.id=this.activateRoute.snapshot.params['id'];
    this.lotService.getLotById(this.id).subscribe(resp=>{
      this.lot=resp;
      this.imagePath=this.lot.Image;
    })
  }

  responseLot;
  ngOnInit(){
    this.id=this.activateRoute.snapshot.params['id']
    this.lotService.getLotById(this.id).subscribe(resp=>{
      this.lot=resp;
      this.imagePath=this.lot['image'];
    });
    this.numbers=Array.from(Array(this.lotService.numbersOfImages).keys());
  }

  checkIfAllImagesIsUploaded():boolean{
    for(let i=0;i<this.numbers.length;i++){
      if(this.lot['images']['image'+(i+1)]=='' || this.lot['images']['image'+(i+1)]==undefined){
        return false;
      }
    }
    return true;
  }

  updateLot(){
    if(this.imagePath && this.checkIfAllImagesIsUploaded()==true){
      this.lot['image']=this.imagePath;
      this.lot['user']=null;
      this.lot['lotState']=null;
      this.lotService.updateLot(this.lot).subscribe(response=>{
      this.toastrService.success("Lot is updated");
      this.saved = true;
      this.router.navigate(['userlots'])
    },
    ()=>this.toastrService.error("Error!"));
    }
    else{
      this.toastrService.error("Download image!");
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

  response;
  imagePath:string;
  @ViewChild('file') fileInput: any;
  uploadFile(files){
    if(files.length === 0)
        return;
    let uploadApiPhoto='https://localhost:44325/api/upload';
    let fileToUpload=<File>files[0];
    const formData=new FormData();
    formData.append('file',fileToUpload,fileToUpload.name);
    this.httpClient.post(uploadApiPhoto,formData, {reportProgress: true, observe: 'events'}).
    subscribe(event=>
    {
      if (event.type === HttpEventType.Response) {
             this.response=event.body; 
             this.imagePath=(this.response['dbPath']);
             this.toastrService.success('Photo is uploaded!');
             this.fileInput.nativeElement.value = '';
      }
    });
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }

  deleteMainPhoto(){
    if(this.imagePath!==''){
      this.lotService.deletePhoto(this.imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.imagePath='';
      });
    }
  }

  uploadFiles(files,field,number){
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
             this.lot['images'][field]=this.response['dbPath'];
             this.toastrService.success('Photo is uploaded!');
      }
    });
  }

  numbers=[];
  deletePhotoByPath(imagePath:string,number:number){
    if(imagePath!==''){
      this.lotService.deletePhoto(imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.lot['images']['image'+number]='';
        document.getElementById('but-'+number).style.display='block';
      });
    }
  }
}
