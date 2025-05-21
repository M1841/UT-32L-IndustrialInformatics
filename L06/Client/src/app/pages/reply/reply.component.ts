import { Component, computed, inject, signal } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EMPTY, map, switchMap } from "rxjs";

import { ApiService } from "@/services/api/api.service";
import { relativeTime } from "@/utils/relativeTime";

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
      <a class="btn btn-light" href="/reply/create?thread_id={{ threadId() }}">
        New Reply
      </a>

      @for (reply of displayedReplies(); track $index) {
        <div class="container-fluid form-control p-3 d-flex flex-column gap-3">
          <div class="d-flex justify-content-between">
            <p class="m-0">{{ reply.author }} - {{ reply.createdAt }}</p>
          </div>
          <p class="m-0">{{ reply.content }}</p>
          @if (reply.author == nickname()) {
            <div class="d-flex justify-content-between">
              <div class="btn-group">
                <a
                  class="btn btn-light"
                  href="/reply/edit?reply_id={{ reply.id }}"
                >
                  Edit
                </a>
                <a
                  class="btn btn-light"
                  href="/reply/delete?reply_id={{ reply.id }}"
                >
                  Delete
                </a>
              </div>
            </div>
          }
        </div>
      }
      @if (this.displayedReplies().length == 0) {
        <p class="text-center pt-2 text-muted">No replies found</p>
      }
    </div>
  `,
  styles: ``,
})
export class ReplyComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly threadId = signal<string>("");
  readonly replies = signal<Reply[]>([]);
  readonly displayedReplies = signal<Reply[]>([]);

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

  ngOnInit() {
    this.route.queryParams
      .pipe(
        map((params) => params["thread_id"]),
        switchMap((threadId) => {
          if (!threadId) {
            this.router.navigate(["/thread"]);
            return EMPTY;
          } else {
            this.threadId.set(threadId);
            return this.api.get<Reply[]>(`thread/${threadId}/replies`);
          }
        })
      )
      .subscribe((res) => {
        this.replies.set(
          res.body?.map((reply) => {
            return {
              ...reply,
              createdAt: relativeTime(new Date(reply.createdAt)),
            };
          }) ?? []
        );
        this.displayedReplies.set(this.replies());
      });
  }
}
