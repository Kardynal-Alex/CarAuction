import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs/operators';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { CommentViewModel } from 'src/app/generated-models/comment-view-model';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';
import { AuthService } from 'src/app/services/auth.service';
import { CommentService } from 'src/app/services/comment.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.less']
})
export class CommentsComponent implements OnInit {

  @Input() public lot: LotViewModel;
  @Input() public userId: string;
  @Input() public comments: CommentViewModel[];
  @Input() public filtredComments: CommentViewModel[];
  public id: number;
  public userName: string;
  public userSurname: string;
  constructor(
    private commentService: CommentService,
    private toastrService: ToastrService,
    private activateRoute: ActivatedRoute,
    private localStorage: LocalStorageService,
    private authService: AuthService
  ) { }

  public ngOnInit() {
    this.id = this.activateRoute.snapshot.params['id'];
    this.getUserId();
  }

  public newest() {
    this.filtredComments = this.comments;
  }

  public sellerCommnets() {
    this.filtredComments = this.comments.filter(x => x.userId === this.lot.userId);
  }

  public bidHistory() {
    this.filtredComments = this.comments.filter(x => x.isBid === true);
  }

  public createComment(form: NgForm) {
    if (this.userId != null) {
      const comment: CommentViewModel = {
        id: Guid.create().toString(),
        author: `${this.userName} ${this.userSurname}`,
        text: form.value.text,
        dateTime: new Date(Date.now()),
        lotId: this.id.toString(),
        userId: this.userId,
        isBid: false
      };
      this.commentService.addComment(comment)
        .subscribe((_) => {
          this.toastrService.success('Comment is added');
          form.resetForm();
          this.getComments(this.id);
        }, (_) => {
          this.toastrService.error('Can not add your comment')
        });
    } else {
      this.toastrService.warning(ErrorMessages.Unauthorized, 'Warning!');
    }
  }

  public deleteComment(commentId: string) {
    this.commentService.deleteCommentById(commentId)
      .subscribe((_) => {
        this.toastrService.success('Comment is deleted!');
        this.getComments(this.id);
      }, (_) => {
        this.toastrService.error(ErrorMessages.Error);
      });
  }

  private getUserId() {
    const token = this.localStorage.get(CommonConstants.JWTToken);
    if (token != null) {
      const payload = this.authService.getPayload();
      this.userName = payload?.name;
      this.userSurname = payload?.surname;
    }
  }

  private getComments(lotId: number) {
    this.commentService.getCommentsByLotId(lotId)
      .pipe(tap((comments) => {
        this.comments = comments;
        this.filtredComments = comments;
      }))
      .subscribe();
  }

}
