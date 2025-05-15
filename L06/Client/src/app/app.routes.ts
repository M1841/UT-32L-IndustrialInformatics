import { Routes } from "@angular/router";

import { HomeComponent } from "@/pages/home/home.component";
import { ThreadComponent } from "@/pages/thread/thread.component";
import { ReplyComponent } from "@/pages/reply/reply.component";

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
  },
  {
    path: "thread",
    component: ThreadComponent,
  },
  {
    path: "reply",
    component: ReplyComponent,
  },
];
