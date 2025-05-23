import { Component, computed, inject, signal } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EMPTY, map, switchMap } from "rxjs";

import { ApiService } from "@/services/api/api.service";

@Component({
  selector: "app-edit-thread",
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

      <button type="submit" class="btn btn-light">Edit</button>
    </form>
  `,
  styles: ``,
})
export class EditThreadComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly id = signal<string>("");
  readonly form = new FormGroup({
    title: new FormControl(""),
    description: new FormControl(""),
  });

  handleSubmit() {
    if (this.form.valid) {
      this.api.put(`thread/${this.id()}`, this.form.value).subscribe({
        next: () => {
          this.router.navigate(["/thread"]);
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
        map((params) => params["thread_id"]),
        switchMap((id) => {
          if (!id) {
            this.router.navigate(["/thread"]);
            return EMPTY;
          } else {
            this.id.set(id);
            return this.api.get<{ result: Thread }>(`thread/${id}`);
          }
        })
      )
      .subscribe((res) => {
        if (!res.body) {
          this.router.navigate(["/thread"]);
        } else {
          const { author, title, description } = res.body.result;
          if (this.nickname() !== author) {
            this.router.navigate(["/"]);
          } else {
            this.form.setValue({ title, description });
          }
        }
      });
  }
}
