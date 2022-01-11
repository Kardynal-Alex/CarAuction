import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs/operators';
import { Lot } from 'src/app/models/lot';
import { CommentService } from 'src/app/services/comment.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { Comment } from '../../models/comment';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  constructor(private commentService:CommentService,
              private toastrService:ToastrService,
              private activateRoute:ActivatedRoute,
              private localStorage:LocalStorageService) { }
  @Input() lot:Lot;
  @Input() userId:string;

  @Input() comments:Comment[];
  @Input() filtredComments:Comment[];
  id: number;
  ngOnInit() {
    this.id=this.activateRoute.snapshot.params['id'];
    this.getUserId();
  }

 getComments(lotId:number){
    this.commentService.getCommentsByLotId(lotId).pipe(tap(comments=>{
      this.comments=comments;
      this.filtredComments=comments;
    })).
    subscribe();
  }

  newest(){
    this.filtredComments=this.comments;
  }

  sellerCommnets(){
    this.filtredComments=this.comments.filter(x=>x['userId']==this.lot['userId']);
  }

  bidHistory(){
    this.filtredComments=this.comments.filter(x=>x['isBid']==true);
  }

  createComment(form:NgForm){
    if(this.userId!=null){
      const comment:Comment={
        Id:Guid.create().toString(),
        Author:this.userName+" "+this.userSurname,
        Text:form.value.Text,
        DateTime:new Date(Date.now()),
        LotId:this.id.toString(),
        UserId:this.userId,
        IsBid:false
      }
      this.commentService.addComment(comment).subscribe(response=>{
        this.toastrService.success("Comment is added");
        form.resetForm();
        this.getComments(this.id);
      },
      error=>{
        this.toastrService.error("Can not add your comment")
      })
    }else{
      this.toastrService.warning("Only for registered user","Warning!");
    }
  }

  deleteComment(commentId:string){
    this.commentService.deleteVommentById(commentId).subscribe(response=>{
      this.toastrService.success("Comment is deleted!");
      this.getComments(this.id);
    },error=>{
      this.toastrService.error("Error!");
    })
  }

  userName:string;
  userSurname:string;
  getUserId(){
    var token=this.localStorage.get('token');
    if(token!=null){
      var payload=JSON.parse(window.atob(token.split('.')[1]));
      this.userName=payload.name;
      this.userSurname=payload.surname;
    }
  }

}
