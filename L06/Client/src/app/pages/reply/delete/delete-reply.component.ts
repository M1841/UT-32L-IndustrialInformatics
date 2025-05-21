import { ApiService } from "@/services/api/api.service";
import { Component, computed, inject, signal } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { map, switchMap, EMPTY } from "rxjs";

@Component({
  selector: "app-delete-reply",
  imports: [],
  template: `
    <div class="container-fluid d-flex flex-column gap-2">
      <p>Are you sure you want to delete the reply?</p>
      <button (click)="handleClick()" class="btn btn-danger form-control">
        Yes, delete
      </button>
      <a class="btn btn-light form-control">No, cancel</a>
    </div>
  `,
  styles: ``,
})
export class DeleteReplyComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly threadId = signal<string>("");
  readonly id = signal<string>("");
  readonly title = signal<string>("");

  handleClick() {
    this.api.delete(`reply/${this.id()}`).subscribe({
      next: () => {
        this.router.navigate(["/reply"], {
          queryParams: { thread_id: this.threadId() },
        });
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  ngOnInit() {
    this.route.queryParams
      .pipe(
        map((params) => params["reply_id"]),
        switchMap((id) => {
          if (!id) {
            this.router.navigate(["/thread"]);
            return EMPTY;
          } else {
            this.id.set(id);
            return this.api.get<Reply>(`reply/${id}`);
          }
        })
      )
      .subscribe((res) => {
        if (!res.body) {
          this.router.navigate(["/thread"]);
        } else {
          const { author, threadId } = res.body;
          if (this.nickname() !== author) {
            this.router.navigate(["/"]);
          } else {
            this.threadId.set(threadId);
          }
        }
      });
  }
}
