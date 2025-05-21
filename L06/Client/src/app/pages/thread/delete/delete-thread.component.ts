import { Component, computed, inject, signal } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { map, switchMap } from "rxjs";

import { ApiService } from "@/services/api/api.service";

@Component({
  selector: "app-delete-thread",
  imports: [],
  template: `
    <div class="container-fluid d-flex flex-column gap-2">
      <p>
        Are you sure you want to delete "{{ title() }}" and all replies under
        it?
      </p>
      <button (click)="handleClick()" class="btn btn-danger form-control">
        Yes, delete
      </button>
      <a class="btn btn-light form-control">No, cancel</a>
    </div>
  `,
  styles: ``,
})
export class DeleteThreadComponent {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly nickname = computed(this.api.nickname);
  readonly id = signal<string>("");
  readonly title = signal<string>("");

  handleClick() {
    console.log("aaaa");
    this.api.delete(`thread/${this.id()}`).subscribe({
      next: () => {
        this.router.navigate(["/thread"]);
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  ngOnInit() {
    this.route.queryParams
      .pipe(
        map((params) => params["thread_id"] as string),
        switchMap((id) => {
          this.id.set(id);
          return this.api.get<any>(`thread/${id}`);
        })
      )
      .subscribe((res) => {
        const { author, title } = res.body.result;
        if (this.nickname() !== author) {
          this.router.navigate(["/"]);
        } else {
          this.title.set(title);
        }
      });
  }
}
