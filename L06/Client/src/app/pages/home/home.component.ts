import { Component, inject } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { CookieService } from "ngx-cookie-service";

@Component({
  selector: "app-home",
  imports: [ReactiveFormsModule],
  template: `
    <form
      [formGroup]="form"
      (ngSubmit)="handleSubmit()"
      class="container-fluid d-flex flex-column gap-4"
    >
      <label>
        Nickname
        <input formControlName="nickname" required class="form-control" />
      </label>

      <button type="submit" class="btn btn-light">Continue</button>
    </form>
  `,
  styles: ``,
})
export class HomeComponent {
  readonly form = new FormGroup({
    nickname: new FormControl(),
  });

  handleSubmit() {
    if (this.form.valid) {
      this.cookies.set("nickname", this.form.value.nickname);
      this.router.navigate(["/thread"]);
    }
  }

  private router = inject(Router);
  private cookies = inject(CookieService);
}
