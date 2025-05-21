import { ApiService } from "@/services/api/api.service";
import { Component, computed, inject, signal } from "@angular/core";
import { FormGroup, FormControl, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { map, switchMap, EMPTY } from "rxjs";

@Component({
  selector: "app-edit-reply",
  imports: [ReactiveFormsModule],
  template: `
    <form
      [formGroup]="form"
      (ngSubmit)="handleSubmit()"
      class="container-fluid d-flex flex-column gap-4"
    >
      <label>
        Content
        <textarea
          formControlName="content"
          required
          class="form-control"
        ></textarea>
      </label>

      <button type="submit" class="btn btn-light">Edit</button>
    </form>
  `,
  styles: ``,
})
export class EditReplyComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly threadId = signal<string>("");
  readonly id = signal<string>("");
  readonly form = new FormGroup({
    content: new FormControl(""),
  });

  handleSubmit() {
    if (this.form.valid) {
      this.api.put(`reply/${this.id()}`, this.form.value).subscribe({
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
          const { author, content, threadId } = res.body;
          if (this.nickname() !== author) {
            this.router.navigate(["/"]);
          } else {
            this.form.setValue({ content });
            this.threadId.set(threadId);
          }
        }
      });
  }
}
