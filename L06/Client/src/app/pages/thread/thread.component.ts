import { ApiService } from "@/services/api/api.service";
import { Component, computed, inject, signal } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";

import { relativeTime as utilsRelativeTime } from "@/utils/relativeTime";

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
      <a class="btn btn-light">New Thread</a>

      @for (thread of displayedThreads(); track $index) {
        <div class="container-fluid form-control p-3 d-flex flex-column gap-3">
          <div class="d-flex justify-content-between">
            <h5 class="m-0">{{ thread.title }}</h5>
            <p class="m-0">
              {{ thread.author }} - {{ relativeTime(thread.createdAt) }}
            </p>
          </div>
          <p class="m-0">{{ thread.description }}</p>
          <div class="d-flex justify-content-between">
            <a href="/reply?threadId={{ thread.id }}" class="btn btn-light">
              View {{ thread.replies.length }} repl{{
                thread.replies.length == 1 ? "y" : "ies"
              }}
            </a>
            @if (thread.author == nickname()) {
              <div class="btn-group">
                <a class="btn btn-light">Edit</a>
                <a class="btn btn-light">Delete</a>
              </div>
            }
          </div>
        </div>
      }
      @if (this.displayedThreads().length == 0) {
        <p>No threads found</p>
      }
    </div>
  `,
  styles: ``,
})
export class ThreadComponent {
  private api = inject(ApiService);

  readonly nickname = computed(this.api.nickname);
  readonly threads = signal<Thread[]>([]);
  readonly displayedThreads = signal<Thread[]>([]);

  readonly searchForm = new FormGroup({
    query: new FormControl(),
  });
  handleSearch() {
    const query = this.searchForm.value.query;
    if (!!query) {
      this.displayedThreads.set(
        this.threads().filter(
          (thread) =>
            thread.author.toLowerCase().includes(query.toLowerCase()) ||
            thread.description.toLowerCase().includes(query.toLowerCase()) ||
            thread.title.toLowerCase().includes(query.toLowerCase())
        )
      );
    } else {
      this.displayedThreads.set(this.threads());
    }
  }

  relativeTime(time: string) {
    return utilsRelativeTime(new Date(time));
  }

  ngOnInit() {
    this.api.get<Thread[]>("thread").subscribe((res) => {
      this.threads.set(res.body ?? []);
      this.displayedThreads.set(this.threads());
    });
  }
}
