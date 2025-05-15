import { HttpClient } from "@angular/common/http";
import { computed, inject, Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";

@Injectable({
  providedIn: "root",
})
export class ApiService {
  readonly isAuthenticated = computed(() => !!this.cookies.get("nickname"));
  readonly nickname = computed(() => this.cookies.get("nickname"));

  get<Res>(endpoint: string, params: string = "") {
    return this.http.get<Res>(
      `${this.url}/${endpoint}${params}`,
      this.options()
    );
  }

  post<Res, Req>(endpoint: string, body: Req) {
    return this.http.post<Res>(`${this.url}/${endpoint}`, body, this.options());
  }

  private readonly url = "http://localhost:5190";
  private options() {
    return {
      headers: {
        Authorization: `Bearer ${this.cookies.get("access_token")}`,
      },
      observe: "response" as const,
    };
  }

  private readonly cookies = inject(CookieService);
  private readonly http = inject(HttpClient);
}
