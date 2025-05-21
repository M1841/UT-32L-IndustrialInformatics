import { ApiService } from "@/services/api/api.service";
import { Component, computed, inject, signal } from "@angular/core";
import { FormGroup, FormControl, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { map } from "rxjs";

@Component({
  selector: "app-create-reply",
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

      <button type="submit" class="btn btn-light">Add</button>
    </form>
  `,
  styles: ``,
})
export class CreateReplyComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly threadId = signal<string>("");
  readonly form = new FormGroup({
    content: new FormControl(""),
  });

  handleSubmit() {
    if (this.form.valid) {
      this.api
        .post("reply", {
          threadId: this.threadId(),
          author: this.nickname(),
          content: this.form.value.content,
        })
        .subscribe({
          next: () => {
            this.router.navigate([`/reply?thread_id=${this.threadId()}`]);
          },
          error: (err) => {
            console.error(err);
          },
        });
    }
  }
  ngOnInit() {
    this.route.queryParams
      .pipe(map((params) => params["thread_id"]))
      .subscribe((threadId) => {
        if (!threadId) {
          this.router.navigate(["/thread"]);
        } else {
          this.threadId.set(threadId);
        }
      });
  }
}
