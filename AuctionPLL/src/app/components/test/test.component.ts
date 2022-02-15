import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Images } from 'src/app/models/images';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.less']
})
export class TestComponent implements OnInit {

  constructor(private lotService: LotService,
    private httpClient: HttpClient,
    private toastrService: ToastrService) {
    this.myForm = new FormGroup({
      Image1: new FormControl(''),
      Image2: new FormControl(''),
      Image3: new FormControl('')
    });
  }
  numbers = [1, 2, 3];
  ngOnInit() {
    this.images = {
      Image1: '', Image2: '', Image3: '', Image4: '', Image5: '', Image6: '', Image7: '', Image8: '', Image9: '', Id: 0
    };
  }
  myForm: FormGroup;
  images: Images;
  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  onSubmit() {
    /*  if(!this.myForm.invalid){
 
     } */
    console.log("image1", this.myForm.controls['Image1'].value);
  }

  response;
  onFileChange(event, field, number) {
    /* if(files.length === 0)
      return; */
    let uploadApiPhoto = 'https://localhost:44325/api/upload';
    //let fileToUpload=<File>files[0];
    let fileToUpload = event.target.files[0];
    let formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          this.response = event.body;
          //this.myForm.controls[field]=this.response['dbPath'];
          this.myForm.patchValue({ field: this.response['dbPath'] });
          //console.log(this.response['dbPath']);
          this.images[field] = event;
          console.log(this.myForm.controls[field]);
          this.toastrService.success('Photo is uploaded!');
        }
      });
  }


  deletePhotoByPath(imagePath: string, field: string) {
    if (imagePath !== '') {
      this.lotService.deletePhoto(imagePath).subscribe(response => {
        this.toastrService.success("Photo is deleted");
        //console.log(this.myForm.get(field));
        //this.myForm.patchValue({field:''});
        this.myForm.controls[field] = new FormControl('');
        console.log(field, this.myForm.controls[field]);
        //this.myForm.controls[field]=new FormControl('');
        //document.getElementById('but-'+number).style.display='block';
      });
    }
  }
  ////////////////////////////////////////////////////////
  /*   images:Images;
    createLot(form:NgForm){
  
    }
  
    upload(files, field,number){
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
              console.log(this.response['dbPath']);
              //document.getElementById('but-'+number).style.display='none';
              this.images[field]=this.response['dbPath'];
              this.toastrService.success('Photo is uploaded!');
        }
      });
    }
  
    deletePhoto(imagePath:string,field:string){
      if(imagePath!==''){
        this.lotService.deletePhoto(imagePath).subscribe(response=>{
          this.toastrService.success("Photo is deleted");
          this.images[field]='';
          //this.myForm.controls[field]=new FormControl('');
          //document.getElementById('but-'+number).style.display='block';
        });
      }
    } */



}
