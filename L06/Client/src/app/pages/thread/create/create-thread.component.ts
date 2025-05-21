import { Component, computed, inject } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";

import { ApiService } from "@/services/api/api.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-create-thread",
  imports: [ReactiveFormsModule],
  template: `
    <form
      [formGroup]="form"
      (ngSubmit)="handleSubmit()"
      class="container-fluid d-flex flex-column gap-4"
    >
      <label>
        Title
        <input formControlName="title" required class="form-control" />
      </label>

      <label>
        Description
        <textarea formControlName="description" class="form-control"></textarea>
      </label>

      <button type="submit" class="btn btn-light">Add</button>
    </form>
  `,
  styles: ``,
})
export class CreateThreadComponent {
  private api = inject(ApiService);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly form = new FormGroup({
    title: new FormControl(""),
    description: new FormControl(""),
  });

  handleSubmit() {
    if (this.form.valid) {
      this.api
        .post("thread", {
          author: this.nickname(),
          title: this.form.value.title,
          description: this.form.value.description,
        })
        .subscribe({
          next: () => {
            this.router.navigate(["/thread"]);
          },
          error: (err) => {
            console.error(err);
          },
        });
    }
  }
}
