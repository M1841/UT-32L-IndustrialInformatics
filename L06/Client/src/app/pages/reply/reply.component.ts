import { ApiService } from "@/services/api/api.service";
import { Component, computed, inject, signal } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";

import { relativeTime as utilsRelativeTime } from "@/utils/relativeTime";
import { ActivatedRoute } from "@angular/router";
import { map, pipe, switchMap, tap } from "rxjs";

@Component({
  selector: "app-thread",
  imports: [ReactiveFormsModule],
  template: `
    <div class="container-fluid d-flex flex-column gap-2">
      <form [formGroup]="searchForm" (input)="handleSearch()">
        <input
          formControlName="query"
          class="form-control"
          placeholder="Search"
        />
      </form>
      <a class="btn btn-light">New Reply</a>

      @for (reply of displayedReplies(); track $index) {
        <div class="container-fluid form-control p-3 d-flex flex-column gap-3">
          <div class="d-flex justify-content-between">
            <p class="m-0">
              {{ reply.author }} - {{ relativeTime(reply.createdAt) }}
            </p>
          </div>
          <p class="m-0">{{ reply.content }}</p>
          @if (reply.author == nickname()) {
            <div class="d-flex justify-content-between">
              <div class="btn-group">
                <a class="btn btn-light">Edit</a>
                <a class="btn btn-light">Delete</a>
              </div>
            </div>
          }
        </div>
      }
      @if (this.displayedReplies().length == 0) {
        <p>No replies found</p>
      }
    </div>
  `,
  styles: ``,
})
export class ReplyComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);

  readonly nickname = computed(this.api.nickname);
  readonly replies = signal<any[]>([]);
  readonly displayedReplies = signal<any[]>([]);

  readonly searchForm = new FormGroup({
    query: new FormControl(),
  });
  handleSearch() {
    const query = this.searchForm.value.query;
    if (!!query) {
      this.displayedReplies.set(
        this.replies().filter(
          (reply) =>
            reply.author.toLowerCase().includes(query.toLowerCase()) ||
            reply.content.toLowerCase().includes(query.toLowerCase())
        )
      );
    } else {
      this.displayedReplies.set(this.replies());
    }
  }

  relativeTime(time: string) {
    return utilsRelativeTime(new Date(time));
  }

  ngOnInit() {
    this.route.queryParams
      .pipe(
        switchMap((params) => {
          return this.api.get<any[]>(`reply/${params["threadId"]}`);
        })
      )
      .subscribe((res) => {
        this.replies.set(res.body ?? []);
        this.displayedReplies.set(this.replies());
      });
  }
}
