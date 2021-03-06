import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component } from '@angular/core';
import { UntypedFormArray, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.less']
})
export class TestComponent {

  get imageArray() {
    return this.imageForm.get('images') as UntypedFormArray;
  }

  t() { return this.imageForm.get('images') as UntypedFormArray; }

  constructor(
    private lotService: LotService,
    private httpClient: HttpClient,
    private toastrService: ToastrService,
    public fb: UntypedFormBuilder
  ) {
    /*   this.myForm = new FormGroup({
        image1: new FormControl(null),
        image2: new FormControl(null),
        image3: new FormControl(null)
      }); */
    this.imageForm = this.fb.group({
      images: this.fb.array([])
    })

  }
  numbers = [1, 2, 3];
  /*  ngOnInit() {
     /* this.images = {
       image1: '', image2: '', image3: '', image4: '', image5: '', image6: '', image7: '', image8: '', image9: '', id: 0
     }; 
   }
  */
  addImage() {
    this.imageArray.push(new UntypedFormControl(null))
    console.log(this.imageForm.value)
    //(<FormArray>this.myForm.controls["images"]).push(new FormControl());
  }

  /*   myForm: FormGroup; */
  imageForm: UntypedFormGroup;
  /* images: Images; */
  public createImgPath(serverPath: string) {
    if (!!serverPath)
      return this.lotService.createImgPath(serverPath);
  }

  onSubmit() {
  }

  response;
  onFileChange(event, field, number) {
    if (event.length === 0)
      return;
    let uploadApiPhoto = 'https://localhost:44325/api/upload';
    //let fileToUpload=<File>files[0];
    let fileToUpload = event.target.files[0];
    let formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' }).
      subscribe(event => {
        if (event.type === HttpEventType.Response) {
          this.response = event.body;
          console.log(this.response['dbPath']);

          const myForm = this.imageArray.at(number);
          myForm.patchValue(this.response['dbPath']);
          this.toastrService.success('Photo is uploaded!');
        }
      });
  }

  removeImage(index) {
    console.log("???? ~ file: test.component.ts ~ line 84 ~ TestComponent ~ removeImage ~ index", index)

    this.imageArray.removeAt(index);
    this.imageForm.markAsDirty();
  }

  deletePhotoByPath(imagePath: string, number: any) {
    if (!!imagePath) {
      this.lotService.deletePhoto(imagePath).subscribe(response => {
        this.toastrService.success("Photo is deleted");
        const myForm = this.imageArray.at(number);
        myForm.patchValue(null);
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
